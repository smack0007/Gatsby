using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatsby
{
    public abstract class Include : GatsbyRazorTemplate
    {
        public dynamic Model
        {
            get;
            private set;
        }

        public string Run(RazorRenderer razorRenderer, Site site, dynamic model)
        {            
            this.InitGatsbyRazorTemplate(razorRenderer, site);

            this.Model = model;

            return this.ExecuteTemplate();
        }
    }
}
