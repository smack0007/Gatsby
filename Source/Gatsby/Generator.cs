using System;

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
