using RazorBurn;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatsby
{
    public abstract class Post : GatsbyRazorTemplate
    {
        string baseUrl;

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
            set;
        }

        public string Layout
        {
            get;
            set;
        }

        public string Permalink
        {
            get;
            set;
        }

        public DateTime Date
        {
            get;
            set;
        }

        public bool Published
        {
            get;
            set;
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

        public DynamicDictionary Data
        {
            get;
            private set;
        }

        public Post()
        {
            this.Published = true;
            this.Categories = new List<string>();
            this.Tags = new List<string>();
            this.Data = new DynamicDictionary();
        }
        
        internal void Run(MarkdownTransformer markdownTransformer, string relativePath, Site site)
        {
            if (this.Content != null)
                throw new InvalidOperationException("Template has already been run.");

            this.RelativePath = relativePath;
            this.Init(site);
                        
            this.Content = this.ExecuteTemplate();
            this.Content = markdownTransformer.Transform(this.Content);
        }
    }
}
