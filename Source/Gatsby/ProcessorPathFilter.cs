using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatsby
{
    public class ProcessorPathFilter : IProcessorPathFilter
    {
        public bool ShouldProcess(string path)
        {
            if (path.StartsWith("_"))
                return false;

            return true;
        }
    }
}
