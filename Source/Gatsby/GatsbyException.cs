using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatsby
{
    public class GatsbyException : Exception
    {
        public GatsbyException(string message)
            : base(message)
        {
        }
    }
}
