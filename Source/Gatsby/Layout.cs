using RazorBurn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatsby
{
    public abstract class Layout : GatsbyRazorTemplate
    {
        public string Content
        {
            get;
            private set;
        }

        public dynamic Model
        {
            get;
            private set;
        }

        public string Parent
        {
            get;
            set;
        }

        public string Run(string content, SiteContent page, RazorRenderer razorRenderer, Site site)
        {
            this.Content = content;
            this.Model = new DynamicValue(page);

            this.InitGatsbyRazorTemplate(razorRenderer, site);

            return this.ExecuteTemplate();
        }
    }
}
