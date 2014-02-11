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
    }
}
