using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatsby
{
    public class SiteBuilder
    {
        SourceFileEnumerator sourceFileEnumerator;
        PluginCompiler pluginCompiler;
        RazorRenderer razorRenderer;
        Logger logger;

        public SiteBuilder(
            SourceFileEnumerator sourceFileEnumerator,
            PluginCompiler pluginCompiler,
            RazorRenderer razorRenderer,
            Logger logger)
        {
            this.sourceFileEnumerator = sourceFileEnumerator;
            this.pluginCompiler = pluginCompiler;
            this.razorRenderer = razorRenderer;
            this.logger = logger;
        }

        public void Build(Config config)
        {
            SourceFiles sourceFiles = this.sourceFileEnumerator.Enumerate(config.Source);
                                               
            if (Directory.Exists(config.Destination))
                Directory.Delete(config.Destination, true);

            Directory.CreateDirectory(config.Destination);

            this.pluginCompiler.RegisterAssemblyResolver();

            try
            {
                foreach (var path in sourceFiles.Plugins)
                {
                    string pluginPath = Path.Combine(config.Destination, Path.GetFileNameWithoutExtension(path.Path) + ".dll");
                    this.pluginCompiler.Compile(path, pluginPath);
                    this.razorRenderer.AddPluginPath(pluginPath);
                }
                
                this.razorRenderer.LoadLayouts(sourceFiles.Layouts);

                Site site = new Site();

                foreach (var path in sourceFiles.Posts)
                {
                    var post = this.razorRenderer.RenderPost(path, site);
                    site.Posts.Add(post);
                }

                foreach (var path in sourceFiles.Pages)
                {
                    var page = this.razorRenderer.RenderPage(path, site);
                    site.Pages.Add(page);
                }

                foreach (var post in site.Posts)
                {
                    string content = this.razorRenderer.LayoutContent(post.Layout, post.Content, post, site);

                    string destination = Path.Combine(config.Destination, post.Permalink);

                    string directory = Path.GetDirectoryName(destination);
                    if (!string.IsNullOrEmpty(directory))
                        Directory.CreateDirectory(directory);

                    File.WriteAllText(destination, content);
                }

                foreach (var page in site.Pages)
                {
                    string content = this.razorRenderer.LayoutContent(page.Layout, page.Content, page, site);

                    string destination = Path.Combine(config.Destination, page.Permalink);

                    string directory = Path.GetDirectoryName(destination);
                    if (!string.IsNullOrEmpty(directory))
                        Directory.CreateDirectory(directory);

                    File.WriteAllText(destination, content);
                }

                foreach (var staticFile in sourceFiles.StaticFiles)
                {
                    string destination = Path.Combine(config.Destination, staticFile.RelativePath);

                    string directory = Path.GetDirectoryName(destination);
                    if (!string.IsNullOrEmpty(directory))
                        Directory.CreateDirectory(directory);

                    File.Copy(staticFile.Path, destination);
                }

                this.pluginCompiler.DeleteAll();
            }
            finally
            {
                this.pluginCompiler.UnregisterAssemblyResolver();
            }
        }
    }
}
