using System;
using System.Collections.Generic;
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
            Configuration = AuthorityConfiguration.Default;
            Users = new UserService();
            Domains = new DomainService();
        }

        public static IUserService Users { get; }
        public static IDomainService Domains { get; }

        public static IAuthorityLogger Logger { get; set; }

        public static Dictionary<Guid, IPasswordValidator> PasswordValidators { get; set; }
        public static IAuthorityEmailService EmailService { get; set; }
        public static List<IAccountObserver> Observers { get; internal set; } 

        public static AuthorityConfiguration Configuration { get; set; }

        public static void Init()
        {
            PasswordValidators = new Dictionary<Guid, IPasswordValidator>();
            Observers = new List<IAccountObserver>();
            Observers.Add(new LoggingObserver());

            SetupEnvironment();
        }

        private static void SetupEnvironment()
        {
            (Domains as IInternalDomainService).LoadDomains();

            AuthorityContext context = new AuthorityContext();

            if (Configuration.DomainMode == DomainMode.Multi)
            {
                return;
            }
        }
    }
}
