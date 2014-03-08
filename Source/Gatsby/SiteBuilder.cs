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

        private void WriteContent(Config config, SiteContent page, Site site)
        {
            string content = this.razorRenderer.LayoutContent(page.Layout, page.Content, page, site);

            string destination = Path.Combine(config.Destination, page.Permalink);

            string directory = Path.GetDirectoryName(destination);
            if (!string.IsNullOrEmpty(directory))
                Directory.CreateDirectory(directory);

            File.WriteAllText(destination, content);
        }

        public void Build(Config config)
        {
            SourceFiles sourceFiles = this.sourceFileEnumerator.Enumerate(config.Source, config.ExcludePatterns);
                                               
            if (Directory.Exists(config.Destination))
                Directory.Delete(config.Destination, true);

            Directory.CreateDirectory(config.Destination);

            this.pluginCompiler.RegisterAssemblyResolver();

            try
            {
                Site site = new Site(config.BaseUrl);

                foreach (var path in sourceFiles.Plugins)
                {
                    string pluginPath = Path.Combine(config.Destination, Path.GetFileNameWithoutExtension(path.AbsolutePath) + ".dll");
                    this.pluginCompiler.Compile(path, pluginPath, site.Plugins);
                    this.razorRenderer.AddPluginPath(pluginPath);
                }
                
                this.razorRenderer.LoadLayouts(sourceFiles.Layouts);

                foreach (var path in sourceFiles.Posts)
                {
                    var post = this.razorRenderer.RenderPost(config, path, site);
                    site.Posts.Add(post);
                }

                site.Posts.Sort((x, y) => x.Date.CompareTo(y.Date) * -1);

                foreach (var path in sourceFiles.Pages)
                {
                    var page = this.razorRenderer.RenderPage(config, path, site);
                    site.Pages.Add(page);
                }

                site.Plugins.BeforePagination(site);

                foreach (var path in sourceFiles.Paginators)
                {
                    var pages = this.razorRenderer.RenderPaginator(config, path, site);
                    site.PaginatorPages.AddRange(pages);
                }

                foreach (var post in site.Posts)
                {
                    this.WriteContent(config, post, site);
                }

                foreach (var page in site.Pages)
                {
                    this.WriteContent(config, page, site);
                }

                foreach (var page in site.PaginatorPages)
                {
                    this.WriteContent(config, page, site);
                }

                foreach (var staticFile in sourceFiles.StaticFiles)
                {
                    string destination = Path.Combine(config.Destination, staticFile.RelativePath);

                    string directory = Path.GetDirectoryName(destination);
                    if (!string.IsNullOrEmpty(directory))
                        Directory.CreateDirectory(directory);

                    File.Copy(staticFile.AbsolutePath, destination);
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
