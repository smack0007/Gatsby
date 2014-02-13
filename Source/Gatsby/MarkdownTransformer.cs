using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatsby
{
    public class MarkdownTransformer
    {       
        public MarkdownTransformer()
        {
        }

        public string Transform(string input)
        {
            return Sundown.MoonShine.Markdownify(input, Sundown.MarkdownExtensions.FencedCode);
        }
    }
}
