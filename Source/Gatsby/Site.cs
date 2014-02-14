using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatsby
{
    public class Site
    {        
        public List<Post> Posts
        {
            get;
            private set;
        }

        public List<Page> Pages
        {
            get;
            private set;
        }

        public Site()
        {
            this.Posts = new List<Post>();
            this.Pages = new List<Page>();
        }
    }
}
