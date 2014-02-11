﻿using System;
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
        RazorCompiler compiler;
        Dictionary<string, Layout> layouts;

        MarkdownTransformer markdownTransformer;

        public RazorRenderer(MarkdownTransformer markdownTransformer)
        {
            this.compiler = new RazorCompiler(RazorLanguage.CSharp);
            this.layouts = new Dictionary<string, Layout>();

            this.markdownTransformer = markdownTransformer;
        }

        public void LoadLayouts(IEnumerable<SourceFilePath> layoutPaths)
        {
            foreach (var path in layoutPaths)
            {
                try
                {
                    var layout = this.compiler.Compile<Layout>(File.ReadAllText(path.Path));
                    layouts.Add(Path.GetFileNameWithoutExtension(path.RelativePath), layout);
                }
                catch (RazorCompilationException ex)
                {
                    throw new GatsbyException(string.Format("Failed while compiling layout {0}:\n\t{1}", path.Path, string.Join("\n\t", ex.Errors)));
                }
            }
        }

        public Post RenderPost(SourceFilePath path, Site site)
        {
            Post post = null;

            try
            {
                post = this.compiler.Compile<Post>(File.ReadAllText(path.Path));
                post.Run(this.markdownTransformer, site);                
            }
            catch (RazorCompilationException ex)
            {
                throw new GatsbyException(string.Format("Failed while compiling post {0}:\n\t{1}", path.Path, string.Join("\n\t", ex.Errors)));
            }

            return post;
        }

        public Page RenderPage(SourceFilePath path, Site site)
        {
            Page page = null;

            try
            {
                page = this.compiler.Compile<Page>(File.ReadAllText(path.Path));
                page.Run(this.markdownTransformer, site);
            }
            catch (RazorCompilationException ex)
            {
                throw new GatsbyException(string.Format("Failed while compiling page {0}:\n\t{1}", path.Path, string.Join("\n\t", ex.Errors)));
            }

            return page;
        }

        public string LayoutContent(string layoutName, string content, Post page, Site site)
        {
            var layout = this.layouts[layoutName];

            content = layout.Run(content, page, site);

            if (string.IsNullOrEmpty(layout.Parent))
                return content;
                
            return this.LayoutContent(layout.Parent, content, page, site);
        }
    }
}