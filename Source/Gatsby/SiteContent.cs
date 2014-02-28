using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatsby
{
    public abstract class SiteContent : GatsbyRazorTemplate
    {
        public string RelativePath
        {
            get;
            private set;
        }

        public string Content
        {
            get;
            private set;
        }

        public string Title
        {
            get;
            protected set;
        }

        public string Layout
        {
            get;
            protected set;
        }

        public string Permalink
        {
            get;
            protected set;
        }

        public DateTime Date
        {
            get;
            protected set;
        }

        public bool Published
        {
            get;
            protected set;
        }

        public bool IsMarkdown
        {
            get;
            protected set;
        }

        public DynamicDictionary Data
        {
            get;
            private set;
        }

        public SiteContent()
            : base()
        {
            this.Published = true;
            this.IsMarkdown = true;
            this.Data = new DynamicDictionary();
        }

        internal virtual void Run(Config config, MarkdownTransformer markdownTransformer, string relativePath, Site site)
        {
            if (this.Content != null)
                throw new InvalidOperationException("Template has already been run.");

            this.RelativePath = relativePath;
            this.InitGatsbyRazorTemplate(site);

            this.Content = this.ExecuteTemplate();

            if (this.IsMarkdown)
                this.Content = markdownTransformer.Transform(this.Content);
        }
    }
}
