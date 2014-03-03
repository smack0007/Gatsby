using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatsby
{
    public interface IBeforePaginationHook
    {
        void BeforePagination(Site site);
    }
}
