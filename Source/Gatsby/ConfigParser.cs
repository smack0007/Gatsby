using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Gatsby
{
    public class ConfigParser
    {
        public Config Parse(string configPath)
        {
            XDocument doc = XDocument.Load(configPath);

            Config config = new Config();

            config.Source = doc.Root.GetValue("Source");
            config.Destination = doc.Root.GetValue("Destination");

            return config;
        }
    }
}
