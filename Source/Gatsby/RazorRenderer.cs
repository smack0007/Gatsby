using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RazorBurn;
using System.IO;

namespace Gatsby
{
    public class RazorRenderer
    {
        MarkdownTransformer markdownTransformer;
        Logger logger;

        RazorCompiler compiler;
        Dictionary<string, Include> includes;
        Dictionary<string, Layout> layouts;
                
        public RazorRenderer(MarkdownTransformer markdownTransformer, Logger logger)
        {
            this.markdownTransformer = markdownTransformer;
            this.logger = logger;

            this.compiler = new RazorCompiler();
            this.compiler.NamespaceImports.Add("System.Linq");
            this.compiler.NamespaceImports.Add("Gatsby");

            this.includes = new Dictionary<string, Include>();
            this.layouts = new Dictionary<string, Layout>();
        }

        public void AddPluginPath(string path)
        {
            this.compiler.ReferencedAssemblies.AddFile(Path.GetFullPath(path));
        }

        public void LoadIncludes(IEnumerable<SourceFilePath> includePaths)
        {
            foreach (var path in includePaths)
            {
                try
                {
                    var include = this.compiler.Compile<Include>(File.ReadAllText(path.AbsolutePath));
                    includes.Add(Path.GetFileNameWithoutExtension(path.RelativePath), include);
                }
                catch (RazorCompilationException ex)
                {
                    throw new GatsbyException(string.Format("Failed while compiling include {0}:\n\t{1}", path.AbsolutePath, string.Join("\n\t", ex.Errors)));
                }
            }
        }

        public void LoadLayouts(IEnumerable<SourceFilePath> layoutPaths)
        {
            foreach (var path in layoutPaths)
            {
                try
                {
                    var layout = this.compiler.Compile<Layout>(File.ReadAllText(path.AbsolutePath));
                    layouts.Add(Path.GetFileNameWithoutExtension(path.RelativePath), layout);
                }
                catch (RazorCompilationException ex)
                {
                    throw new GatsbyException(string.Format("Failed while compiling layout {0}:\n\t{1}", path.AbsolutePath, string.Join("\n\t", ex.Errors)));
                }
            }
        }
                
        public Post RenderPost(Config config, SourceFilePath path, Site site)
        {
            Post post = null;

            try
            {
                post = this.compiler.Compile<Post>(File.ReadAllText(path.AbsolutePath));
                post.Run(config, this.markdownTransformer, path.RelativePath, this, site);                
            }
            catch (RazorCompilationException ex)
            {
                throw new GatsbyException(string.Format("Failed while compiling post {0}:\n\t{1}", path.AbsolutePath, string.Join("\n\t", ex.Errors)));
            }

            return post;
        }

        public Page RenderPage(Config config, SourceFilePath path, Site site)
        {
            Page page = null;

            try
            {
                page = this.compiler.Compile<Page>(File.ReadAllText(path.AbsolutePath));
                page.Run(config, this.markdownTransformer, path.RelativePath, this, site);
            }
            catch (RazorCompilationException ex)
            {
                throw new GatsbyException(string.Format("Failed while compiling page {0}:\n\t{1}", path.AbsolutePath, string.Join("\n\t", ex.Errors)));
            }

            return page;
        }

        public List<PaginatorPage> RenderPaginator(Config config, SourceFilePath path, Site site)
        {
            List<PaginatorPage> pagintors = new List<PaginatorPage>();
            RazorTemplateFactory<PaginatorPage> factory = null;

            try
            {
                factory = this.compiler.CompileFactory<PaginatorPage>(File.ReadAllText(path.AbsolutePath));
            }
            catch (RazorCompilationException ex)
            {
                throw new GatsbyException(string.Format("Failed while compiling paginator {0}:\n\t{1}", path.AbsolutePath, string.Join("\n\t", ex.Errors)));
            }

            int pageNumber = 1;

            while (true)
            {
                var template = factory.Create();
                template.PageNumber = pageNumber;
                template.Run(config, this.markdownTransformer, path.RelativePath, this, site);

                pagintors.Add(template);

                if (template.PaginationFinished)
                    break;

                pageNumber++;
            }

            return pagintors;
        }

        public string RenderInclude(string includeName, Site site, dynamic model)
        {
            if (!this.includes.ContainsKey(includeName))
                throw new GatsbyException(string.Format("Include \"{0}\" was not found.", includeName));

            var include = this.includes[includeName];

            return include.Run(this, site, model);
        }

        public string LayoutContent(string layoutName, string content, SiteContent page, Site site)
        {
            if (!this.layouts.ContainsKey(layoutName))
                throw new GatsbyException(string.Format("Layout \"{0}\" was not found.", layoutName));

            var layout = this.layouts[layoutName];

            content = layout.Run(content, page, this, site);

            if (string.IsNullOrEmpty(layout.Parent))
                return content;
                
            return this.LayoutContent(layout.Parent, content, page, site);
        }
    }
}
