using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatsby
{
    public class Config
    {
        public string Source
        {
            get;
            set;
        }

        public string Destination
        {
            get;
            set;
        }

        public string BaseUrl
        {
            get;
            set;
        }

        public List<string> ExcludePatterns
        {
            get;
            set;
        }

        public string ExcerptSeparator
        {
            get;
            set;
        }

        public Config()
        {
            this.ExcludePatterns = new List<string>();
        }
    }
}
