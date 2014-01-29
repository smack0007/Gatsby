using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatsby
{
    public interface IProcessorPathFilter
    {
        bool ShouldProcess(string path);
    }
}
