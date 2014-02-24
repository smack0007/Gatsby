using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatsby
{
    public abstract class PaginatorPage : Page
    {
        public int PageNumber
        {
            get;
            internal set;
        }

        public bool PaginationFinished
        {
            get;
            protected set;
        }
    }
}
