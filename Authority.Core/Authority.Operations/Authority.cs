using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Authority.DomainModel;
using Authority.EntityFramework;
using Authority.Operations.Configuration;
using Authority.Operations.Developers;
using Serilog;

namespace Authority.Operations
{
    public sealed class Authority
    {
        private const string EventLogName = "AuthorityLogs";

        public static ILogger Logger { get; set; }

        private static AuthorityConfiguration _configuration;
        public static AuthorityConfiguration Configuration
        {
            get
            {
                if (_configuration != null)
                {
                    return _configuration;
                }

                return AuthorityConfiguration.Default;
            }
        }

        public static void Init()
        {
            ReadConfiguration();
            InitLogging();
            SetupEnvironment();
        }

        private static void ReadConfiguration()
        {
            if (File.Exists("authority.json"))
            {
                string json = File.ReadAllText("authority.json");
                _configuration = AuthorityConfiguration.FromJson(json);
            }
        }

        private static void InitLogging()
        {
            AuthorityConfiguration configuration = Configuration;

            LoggerConfiguration loggerConfiguration = new LoggerConfiguration();

            switch (configuration.LogTarget)
            {
                case LogTargetConstants.EventLog:
                    loggerConfiguration = loggerConfiguration.WriteTo.EventLog("Authority");
                    break;
                case LogTargetConstants.File:
                    loggerConfiguration = loggerConfiguration.WriteTo.RollingFile(@"c:\log-{Date}.txt");
                    break;
                default:
                    throw new ArgumentException("Unknown logging target");
            }

            Logger = loggerConfiguration.CreateLogger();
        }

        private static void SetupEnvironment()
        {
            AuthorityContext context = new AuthorityContext();

            if (Configuration.Mode == ModeConstants.Server)
            {
                return;
            }

            Developer admin = context.Developers.FirstOrDefault();

            if (admin == null)
            {
                DeveloperRegistration registerAdmin = new DeveloperRegistration(context, "admin@authority.com", "admin", "p@ssw0rd1");
                admin = registerAdmin.Do().Result;
                registerAdmin.Commit();

                DeveloperActivation activateAdmin = new DeveloperActivation(context, admin.PendingRegistrationId);
                activateAdmin.Do().Wait();
                activateAdmin.Commit();
            }

            List<Product> products = context.Products.ToList();
        }
    }
}
