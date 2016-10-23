using System;
using System.Collections.Generic;
using AuthorityIdentity.Observers;
using AuthorityIdentity.Security;

namespace AuthorityIdentity.Configuration
{
    public enum DomainMode
    {
        Single,
        Multi
    }

    public enum TwoFactorMode
    {
        Optional,
        Strict
    }

    public sealed class AuthorityConfiguration
    {
        public static AuthorityConfiguration Default
        {
            get
            {
                return new AuthorityConfiguration
                {
                    DomainMode = DomainMode.Single,
                    Logger = null,
                    PasswordValidators = new Dictionary<Guid, IPasswordValidator>(),
                    EmailService = null,
                    Observers = new List<IAccountObserver> {  new LoggingObserver() },
                    TwoFactorAuthenticationEnabled = false,
                    TwoFactorMode = TwoFactorMode.Optional,
                    TwoFactorService = null,
                    ExternalIdentityProviders = new List<ExternalIdentityProvider>()
                };
            }
        }

        public DomainMode DomainMode { get; set; }

        public IAuthorityLogger Logger { get; set; }
        public List<ExternalIdentityProvider> ExternalIdentityProviders { get; set; } 
        public Dictionary<Guid, IPasswordValidator> PasswordValidators { get; set; }
        public IAuthorityEmailService EmailService { get; set; }
        public List<IAccountObserver> Observers { get; set; }
        public ITwoFactorService TwoFactorService { get; set; }

        public bool TwoFactorAuthenticationEnabled { get; set; }
        public TwoFactorMode TwoFactorMode { get; set; }
    }
}
