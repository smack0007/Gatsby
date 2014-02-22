using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatsby
{
    public struct SourceFilePath
    {
        public string AbsolutePath
        {
            get;
            set;
        }

        public string RelativePath
        {
            get;
            set;
        }

        public override string ToString()
        {
            return this.AbsolutePath;
        }
    }
}
