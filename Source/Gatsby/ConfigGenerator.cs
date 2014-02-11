using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatsby
{
    public class ConfigGenerator
    {
        public void Generate(string configPath)
        {
            using (Stream stream = this.GetType().Assembly.GetManifestResourceStream("Gatsby.Config.xml"))
            using (StreamReader sr = new StreamReader(stream))
            {
                File.WriteAllText(configPath, sr.ReadToEnd());
            }
        }
    }
}
