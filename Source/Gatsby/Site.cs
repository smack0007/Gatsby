﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatsby
{
    public class Site
    {
        public string BaseUrl
        {
            get;
            private set;
        }

        public List<Post> Posts
        {
            get;
            private set;
        }

        public IEnumerable<string> Categories
        {
            get { return this.Posts.SelectMany(x => x.Categories); }
        }

        public IEnumerable<string> Tags
        {
            get { return this.Posts.SelectMany(x => x.Tags); }
        }

        public List<Page> Pages
        {
            get;
            private set;
        }

        public List<PaginatorPage> PaginatorPages
        {
            get;
            private set;
        }

        public Site()
        {
            this.Posts = new List<Post>();
            this.Pages = new List<Page>();
            this.PaginatorPages = new List<PaginatorPage>();
        }
    }
}
