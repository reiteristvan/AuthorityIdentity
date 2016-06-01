using System;
using System.IO;
using Authority.Operations.Configuration;
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
    }
}
