using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatsby
{
    public class SourceFiles
    {
        public IEnumerable<SourceFilePath> Posts
        {
            get;
            set;
        }

        public IEnumerable<SourceFilePath> Pages
        {
            get;
            set;
        }

        public IEnumerable<SourceFilePath> Layouts
        {
            get;
            set;
        }

        public IEnumerable<SourceFilePath> Includes
        {
            get;
            set;
        }

        public IEnumerable<SourceFilePath> StaticFiles
        {
            get;
            set;
        }
    }
}
