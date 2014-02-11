﻿using RazorBurn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatsby
{
    public abstract class Layout : RazorTemplate
    {
        public string Content
        {
            get;
            private set;
        }

        public Post Page
        {
            get;
            private set;
        }

        public Site Site
        {
            get;
            private set;
        }

        public string Parent
        {
            get;
            set;
        }

        public string Run(string content, Post page, Site site)
        {
            this.Content = content;
            this.Page = page;
            this.Site = site;

            return this.ExecuteTemplate();
        }
    }
}