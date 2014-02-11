using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatsby
{
    public class CommandLineOptions
    {
        public GatsbyAction Action
        {
            get;
            set;
        }

        public string ConfigPath
        {
            get;
            set;
        }
    }
}
