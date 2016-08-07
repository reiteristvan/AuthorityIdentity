using System;
using System.Collections.Generic;
using Authority.Operations.Observers;
using Authority.Operations.Security;

namespace Authority.Operations.Configuration
{
    public enum DomainMode
    {
        Single,
        Multi
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
                    Observers = new List<IAccountObserver> {  new LoggingObserver() }
                };
            }
        }

        public DomainMode DomainMode { get; set; }

        public IAuthorityLogger Logger { get; set; }
        public Dictionary<Guid, IPasswordValidator> PasswordValidators { get; set; }
        public IAuthorityEmailService EmailService { get; set; }
        public List<IAccountObserver> Observers { get; set; }
    }
}
