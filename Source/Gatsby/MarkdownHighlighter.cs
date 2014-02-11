using MarkdownSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatsby
{
    public class MarkdownHighlighter : IMarkdownHighlighter
    {
        public bool Highlight(string text, string syntax, out string highlighted)
        {
            highlighted = string.Format("<pre class=\"brush: {0}\">{1}</pre>", syntax, text);
            return true;
        }
    }
}
