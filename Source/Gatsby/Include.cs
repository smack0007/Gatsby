using System;

namespace Gatsby
{
    public abstract class Include : GatsbyRazorTemplate
    {
		public dynamic Model
        {
            get;
            private set;
        }

        public string Run(RazorRenderer razorRenderer, Site site, object model)
        {            
            this.InitGatsbyRazorTemplate(razorRenderer, site);

            this.Model = new DynamicValue(model);

            return this.ExecuteTemplate();
        }
    }
}
