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
        HttpServer httpServer;
        Logger logger;

        public Program(
            CommandLineOptionsParser optionsParser,
            ConfigGenerator configGenerator,
            ConfigParser configParser,
            SiteBuilder siteBuilder,
            HttpServer httpServer,
            Logger logger)
        {
            this.optionsParser = optionsParser;
            this.configGenerator = configGenerator;
            this.configParser = configParser;
            this.siteBuilder = siteBuilder;
            this.httpServer = httpServer;
            this.logger = logger;
        }

        public int Run(string[] args)
        {
            CommandLineOptions options;

            try
            {
                options = optionsParser.Parse(args);
            }
            catch (GatsbyException ex)
            {
                this.logger.Error(ex.Message);
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
            catch (GatsbyException ex)
            {
                this.logger.Error(ex.Message);
                return 2;
            }

            switch (options.Action)
            {
                case GatsbyAction.Build:
                    this.BuildSite(config);
                    break;

                case GatsbyAction.Serve:
                    this.StartHttpServer(config);
                    break;
            }

            return 0;
        }

        private void BuildSite(Config config)
        {
            try
            {
                this.siteBuilder.Build(config);
            }
            catch (GatsbyException ex)
            {
                this.logger.Error("An error occured while building: {0}", ex.Message);
            }
            catch (Exception ex)
            {
                this.logger.Error("An unexpected error occured while building: {0}", ex.Message);
            }
        }

        private void StartHttpServer(Config config)
        {
            this.httpServer.Start(config.Destination, config.BaseUrl);

            Console.WriteLine("Press any key to stop Http server...");
            Console.ReadKey();
        }

        public static int Main(string[] args)
        {
            ComponentContainer container = new ComponentContainer();

            var program = container.Resolve<Program>();
            return program.Run(args);
        }
    }
}
