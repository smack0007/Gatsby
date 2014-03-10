using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatsby
{
    public abstract class Generator : Page
    {
        public int PageNumber
        {
            get;
            internal set;
        }

        public bool GeneratorFinished
        {
            get;
            protected set;
        }

        public Generator()
            : base()
        {
            this.IsMarkdown = false;
        }
    }
}
