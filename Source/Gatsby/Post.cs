using RazorBurn;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatsby
{
    public abstract class Post : SiteContent
    {
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

        public Post()
        {
            this.Categories = new List<string>();
            this.Tags = new List<string>();
        }
    }
}
