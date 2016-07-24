using System.Collections.Generic;
using System.IO;
using System.Linq;
using Authority.DomainModel;
using Authority.EntityFramework;
using Authority.Operations.Configuration;
using Authority.Operations.Observers;
using Authority.Operations.Security;
using Authority.Operations.Services;

namespace Authority.Operations
{
    public sealed class Authority
    {
        static Authority()
        {
            Users = new UserService();
        }

        public static UserService Users { get; }

        public static IAuthorityLogger Logger { get; set; }

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
