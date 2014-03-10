using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComponentGlue;
using System.Threading;
using System.Globalization;

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

            if (!string.IsNullOrEmpty(config.Culture))
                Thread.CurrentThread.CurrentCulture = new CultureInfo(config.Culture);

            switch (options.Action)
            {
                case GatsbyAction.Build:
                    if (!this.BuildSite(config))
                        return 4;
                    break;

                case GatsbyAction.Serve:
                    this.StartHttpServer(config);
                    break;
            }

            return 0;
        }

        private bool BuildSite(Config config)
        {
            try
            {
                this.siteBuilder.Build(config);
            }
            catch (GatsbyException ex)
            {
                this.logger.Error("An error occured while building: {0}", ex.Message);
                return false;
            }
            catch (Exception ex)
            {
                this.logger.Error("An unexpected error occured while building: {0}", ex.Message);
                return false;
            }

            return true;
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
