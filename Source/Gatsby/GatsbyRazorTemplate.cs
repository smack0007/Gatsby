using RazorBurn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatsby
{
    public abstract class GatsbyRazorTemplate : RazorTemplate
    {
        public Site Site
        {
            get;
            private set;
        }

        internal void Init(Site site)
        {
            this.Site = site;
        }

        protected string BaseUrl(string relativeUrl)
        {
            return this.Site.BaseUrl + "/" + relativeUrl;
        }
    }
}
