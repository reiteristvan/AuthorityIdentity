using System;
using Authority.Operations.Configuration;
using Serilog;

namespace Authority.Operations
{
    public sealed class Authority
    {
        private const string EventLogName = "AuthorityLogs";

        public static ILogger Logger { get; set; }

        public static AuthorityConfiguration Configuration
        {
            get { return AuthorityConfiguration.Default; }
        }

        public static void Init()
        {
            InitLogging();
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
