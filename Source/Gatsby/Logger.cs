using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatsby
{
    public class Logger
    {
        public void Error(string format, params object[] args)
        {
            Console.Error.WriteLine(format, args);
        }
    }
}
