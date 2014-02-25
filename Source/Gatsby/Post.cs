using RazorBurn;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatsby
{
    public abstract class Post : SiteContent
    {
        public string Excerpt
        {
            get;
            private set;
        }

        public List<string> Categories
        {
            get;
            private set;
        }

        public List<string> Tags
        {
            get;
            private set;
        }

        public Post()
        {
            this.Categories = new List<string>();
            this.Tags = new List<string>();
        }

        internal override void Run(Config config, MarkdownTransformer markdownTransformer, string relativePath, Site site)
        {
            base.Run(config, markdownTransformer, relativePath, site);

            if (!string.IsNullOrEmpty(config.ExcerptSeparator))
            {
                if (this.Content.Contains(config.ExcerptSeparator))
                {
                    this.Excerpt = this.Content.Substring(0, this.Content.IndexOf(config.ExcerptSeparator));
                }
                else
                {
                    this.Excerpt = this.Content;
                }
            }
        }
    }
}
