using RazorBurn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Gatsby
{
    public abstract class GatsbyRazorTemplate : RazorTemplate
    {
        RazorRenderer razorRenderer;

        public Site Site
        {
            get;
            private set;
        }
        
        internal void InitGatsbyRazorTemplate(RazorRenderer razorRenderer, Site site)
        {
            this.razorRenderer = razorRenderer;
            this.Site = site;
        }

        protected string Include(string includeName)
        {
            return this.razorRenderer.RenderInclude(includeName, this.Site, null);
        }

        protected string Include(string includeName, object model)
        {
            return this.razorRenderer.RenderInclude(includeName, this.Site, model);
        }

        protected string BaseUrl(string relativeUrl)
        {
            return this.Site.BaseUrl.TrimEnd('/') + '/' + relativeUrl.TrimStart('/');
        }
				
		protected static string UrlSlug(string url)
		{
			return Utility.UrlSlug(url);
		}
    }
}
