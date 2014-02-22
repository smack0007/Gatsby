using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Gatsby
{
    public static class XmlExtensions
    {
        public static string GetValue(this XElement element, XName name)
        {
            XAttribute attribute = element.Attribute(name);

            if (attribute != null)
                return attribute.Value;

            XElement childElement = element.Element(name);

            if (childElement != null)
                return childElement.Value;

            return null;
        }

        public static IEnumerable<XElement> GetChildElements(this XElement element, XName name)
        {
            var element2 = element.Element(name);

            if (element2 == null)
                return Enumerable.Empty<XElement>();

            return element2.Elements();
        }
    }
}
