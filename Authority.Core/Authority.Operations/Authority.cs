using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Authority.DomainModel;
using Authority.EntityFramework;
using Authority.Operations.Configuration;
using Authority.Operations.Observers;
using Authority.Operations.Security;
using Authority.Operations.Services;
using Serilog;

namespace Authority.Operations
{
    public sealed class Authority
    {
        static Authority()
        {
            Users = new UserServices();
        }

        public static UserServices Users { get; }

        private const string EventLogName = "AuthorityLogs";

        public static ILogger Logger { get; set; }

        public static IPasswordValidator PasswordValidator { get; set; }
        public static IAuthorityEmailService EmailService { get; set; }
        public static List<IAccountObserver> Observers { get; internal set; } 

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

            Observers = new List<IAccountObserver>();
            Observers.Add(new LoggingObserver());

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
                    loggerConfiguration = loggerConfiguration.WriteTo.EventLog(EventLogName);
                    break;
                case LogTargetConstants.File:
                    loggerConfiguration = loggerConfiguration.WriteTo.RollingFile(@"log-{Date}.txt");
                    break;
                default:
                    throw new ArgumentException("Unknown logging target");
            }

            Logger = loggerConfiguration.CreateLogger();
        }

        private static void SetupEnvironment()
        {
            AuthorityContext context = new AuthorityContext();

            if (Configuration.DomainMode == DomainModeConstants.Multi)
            {
                return;
            }

            List<Domain> domains = context.Domains.ToList();
        }
    }
}
