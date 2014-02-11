using MarkdownSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatsby
{
    public class MarkdownTransformer
    {
        Markdown markdown;

        public MarkdownTransformer(MarkdownHighlighter highlighter)
        {
            this.markdown = new Markdown(new MarkdownOptions()
            {
            });

            this.markdown.Highlighter = highlighter;
        }

        public string Transform(string input)
        {
            return this.markdown.Transform(input);
        }
    }
}
