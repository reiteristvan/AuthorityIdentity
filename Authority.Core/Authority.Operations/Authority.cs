using System;
using System.Collections.Generic;
using System.Linq;
using Authority.DomainModel;
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

        }

        internal static DomainMode DomainMode { get; set; }
        public static IUserService Users { get; internal set; }
        public static IDomainService Domains { get; internal set; }
        public static IPolicyService Policies { get; internal set; }
        public static IClaimService Claims { get; internal set; }

        public static IAuthorityLogger Logger { get; internal set; }

        public static Dictionary<Guid, IPasswordValidator> PasswordValidators { get; internal set; }
        public static IAuthorityEmailService EmailService { get; internal set; }
        public static List<IAccountObserver> Observers { get; internal set; } 

        internal static bool Initialized;
        internal static object LockObject = new object();
        public static void Init(AuthorityConfiguration configuration)
        {
            lock (LockObject)
            {
                if (configuration == null)
                {
                    configuration = AuthorityConfiguration.Default;
                }

                if (Initialized)
                {
                    return;
                }

                DomainMode = configuration.DomainMode;

                Users = new UserService();
                Domains = new DomainService();
                Policies = new PolicyService();
                Claims = new ClaimService();

                Logger = configuration.Logger;
                PasswordValidators = configuration.PasswordValidators ?? new Dictionary<Guid, IPasswordValidator>();
                Observers = configuration.Observers ?? new List<IAccountObserver>();
                EmailService = configuration.EmailService;

                SetupEnvironment(configuration.DomainMode);

                Initialized = true;
            }
        }

        private static void SetupEnvironment(DomainMode domainMode)
        {
            (Domains as IInternalDomainService).LoadDomains(true);

            if (domainMode == DomainMode.Multi)
            {
                return;
            }

            List<Domain> domains = Domains.All();

            if (domains.Any())
            {
                return;
            }

            Guid domainId = Domains.Create(DomainService.MasterDomainName).Result;
        }
    }
}
