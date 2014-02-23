using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatsby
{
    public class CommandLineOptionsParser
    {
        public CommandLineOptions Parse(string[] args)
        {
            var options = new CommandLineOptions()
                {
                    ConfigPath = @"_Config.xml"
                };

            if (args.Length == 0)
                throw new GatsbyException("Please provide an action to perform.");

            switch (args[0].ToLower())
            {
                case "generate":
                    options.Action = GatsbyAction.Generate;
                    break;

                case "build":
                    options.Action = GatsbyAction.Build;
                    break;

                case "serve":
                    options.Action = GatsbyAction.Serve;
                    break;

                default:
                    throw new GatsbyException("Unknown action.");
            }

            for (int i = 1; i < args.Length; i++)
            {
                switch (args[i].ToLower())
                {
                    case "--config":
                        ThrowIfFlagMissingValue(args, i);
                        options.ConfigPath = args[i + 1];
                        i++;
                        break;
                }
            }
            
            return options;
        }

        private static void ThrowIfFlagMissingValue(string[] args, int flagIndex)
        {
            if (flagIndex + 1 >= args.Length)
                throw new GatsbyException(string.Format("Invalid arguments. Flag {0} missing value.", args[flagIndex]));
        }
    }
}
