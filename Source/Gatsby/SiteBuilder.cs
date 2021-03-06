﻿using System;
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
            string content = page.Content;

            if (!string.IsNullOrEmpty(page.Layout))
                content = this.razorRenderer.LayoutContent(page.Layout, content, page, site);

            string destination = Path.Combine(config.Destination, page.Permalink);

            string directory = Path.GetDirectoryName(destination);
            if (!string.IsNullOrEmpty(directory))
                Directory.CreateDirectory(directory);

            File.WriteAllText(destination, content);
        }

		private void SetDefaultsForContent(SiteContent content)
		{
			if (content.LastUpdate == null)
				content.LastUpdate = content.Date;
		}

        public void Build(Config config)
        {
            SourceFiles sourceFiles = this.sourceFileEnumerator.Enumerate(config);
                                               
            if (Directory.Exists(config.Destination))
                Directory.Delete(config.Destination, true);

            Directory.CreateDirectory(config.Destination);

            this.pluginCompiler.RegisterAssemblyResolver();

            try
            {
                Site site = new Site(config);

                foreach (var path in sourceFiles.Plugins)
                {
                    string pluginPath = Path.Combine(config.Destination, Path.GetFileNameWithoutExtension(path.AbsolutePath) + ".dll");
                    this.pluginCompiler.Compile(path, pluginPath, site.Plugins);
                    this.razorRenderer.AddPluginPath(pluginPath);
                }

                this.razorRenderer.LoadIncludes(sourceFiles.Includes);
                this.razorRenderer.LoadLayouts(sourceFiles.Layouts);

                foreach (var path in sourceFiles.Posts)
                {
                    var post = this.razorRenderer.RenderPost(config, path, site);
					this.SetDefaultsForContent(post);
                    site.Posts.Add(post);
                }

                site.Posts.Sort((x, y) => x.Date.CompareTo(y.Date) * -1);

                foreach (var path in sourceFiles.Pages)
                {
                    var page = this.razorRenderer.RenderPage(config, path, site);
					this.SetDefaultsForContent(page);
                    site.Pages.Add(page);
                }

                site.Plugins.BeforeGenerators(site);

                foreach (var path in sourceFiles.Generators)
                {
                    var pages = this.razorRenderer.RenderPaginator(config, path, site);
					
					foreach (var page in pages)
						this.SetDefaultsForContent(page);
                    
					site.GeneratorPages.AddRange(pages);
                }

				site.Plugins.AfterGenerators(site);

                foreach (var post in site.Posts)
                {
                    this.WriteContent(config, post, site);
                }

                foreach (var page in site.Pages)
                {
                    this.WriteContent(config, page, site);
                }

                foreach (var page in site.GeneratorPages)
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
