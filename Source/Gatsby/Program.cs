using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComponentGlue;

namespace Gatsby
{
    public class Program
    {
        CommandLineOptionsParser optionsParser;
        ConfigGenerator configGenerator;
        ConfigParser configParser;
        SiteBuilder siteBuilder;

        public Program(
            CommandLineOptionsParser optionsParser,
            ConfigGenerator configGenerator,
            ConfigParser configParser,
            SiteBuilder siteBuilder)
        {
            this.optionsParser = optionsParser;
            this.configGenerator = configGenerator;
            this.configParser = configParser;
            this.siteBuilder = siteBuilder;
        }

        public int Run(string[] args)
        {
            CommandLineOptions options;

            try
            {
                options = optionsParser.Parse(args);
            }
            catch (InvalidOperationException ex)
            {
                Console.Error.WriteLine(ex.Message);
                return 1;
            }

            if (options.Action == GatsbyAction.Generate)
            {
                this.configGenerator.Generate(options.ConfigPath);
                return 0;
            }

            Config config;

            try
            {
                config = this.configParser.Parse(options.ConfigPath);
            }
            catch (InvalidOperationException ex)
            {
                Console.Error.WriteLine(ex.Message);
                return 2;
            }

            switch (options.Action)
            {
                case GatsbyAction.Build:
                    this.siteBuilder.Build(config);
                    break;
            }

            return 0;
        }

        public static int Main(string[] args)
        {
            ComponentContainer container = new ComponentContainer();

            var program = container.Resolve<Program>();
            return program.Run(args);
        }
    }
}
