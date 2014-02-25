using System;
using System.Collections.Generic;
using System.IO;
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

            string configDirectory = Path.GetDirectoryName(configPath);

            Config config = new Config();
            
            config.Source =  Path.Combine(configDirectory, doc.Root.GetValue("Source"));
            config.Destination = Path.Combine(configDirectory, doc.Root.GetValue("Destination"));
            config.BaseUrl = doc.Root.GetValue("BaseUrl");
            
            foreach (var excludePattern in doc.Root.GetChildElements("ExcludePatterns"))
            {
                config.ExcludePatterns.Add(excludePattern.Value);
            }

            config.ExcerptSeparator = doc.Root.GetValue("ExcerptSeparator");

            return config;
        }
    }
}
