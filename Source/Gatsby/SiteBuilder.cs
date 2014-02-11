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
        RazorRenderer razorRenderer;

        public SiteBuilder(
            SourceFileEnumerator sourceFileEnumerator,
            RazorRenderer razorRenderer)
        {
            this.sourceFileEnumerator = sourceFileEnumerator;
            this.razorRenderer = razorRenderer;
        }

        public void Build(Config config)
        {
            SourceFiles sourceFiles = this.sourceFileEnumerator.Enumerate(config.Source);

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

            if (Directory.Exists(config.Destination))
                Directory.Delete(config.Destination, true);

            Directory.CreateDirectory(config.Destination);

            foreach (var post in site.Posts)
            {
                string content = this.razorRenderer.LayoutContent(post.Layout, post.Content, post, site);

                string destination = Path.Combine(config.Destination, post.Permalink);

                Directory.CreateDirectory(Path.GetDirectoryName(destination));
                
                File.WriteAllText(destination, content);
            }

            foreach (var page in site.Pages)
            {
                string content = this.razorRenderer.LayoutContent(page.Layout, page.Content, page, site);

                string destination = Path.Combine(config.Destination, page.Permalink);

                Directory.CreateDirectory(Path.GetDirectoryName(destination));

                File.WriteAllText(destination, content);
            }
        }
    }
}
