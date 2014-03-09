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

        protected string Include(string includeName, dynamic model)
        {
            return this.razorRenderer.RenderInclude(includeName, this.Site, model);
        }

        protected string BaseUrl(string relativeUrl)
        {
            return this.Site.BaseUrl.TrimEnd('/') + '/' + relativeUrl.TrimStart('/');
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <remarks>Credit to Joan from StackOverflow: http://stackoverflow.com/questions/2920744/url-slugify-algorithm-in-c </remarks>
        protected static string UrlSlug(string value)
        {
            //First to lower case
            value = value.ToLowerInvariant();

            //Remove all accents
            var bytes = Encoding.GetEncoding("Cyrillic").GetBytes(value);
            value = Encoding.ASCII.GetString(bytes);

            //Replace spaces
            value = Regex.Replace(value, @"\s", "-", RegexOptions.Compiled);

            //Remove invalid chars
            value = Regex.Replace(value, @"[^a-z0-9\s-_]", "", RegexOptions.Compiled);

            //Trim dashes from end
            value = value.Trim('-', '_');

            //Replace double occurences of - or _
            value = Regex.Replace(value, @"([-_]){2,}", "$1", RegexOptions.Compiled);

            return value;
        }
    }
}
