using System;
using System.Collections.Generic;
using System.Linq;
using Authority.DomainModel;
using Authority.EntityFramework;
using Authority.Operations.Configuration;
using Authority.Operations.Observers;

namespace Authority.IntegrationTests
{
    public sealed class AuthorityTestContext : IDisposable
    {
        static AuthorityTestContext()
        {
            AuthorityConfiguration configuration = new AuthorityConfiguration
            {
                DomainMode = DomainMode.Multi,
                Logger = new TestLogger(),
                Observers = new List<IAccountObserver> {  new LoggingObserver() }
            };

            Operations.Authority.Init(configuration);
        }

        public AuthorityTestContext()
        {
            Context = new AuthorityContext();
            Guid domainId = Operations.Authority.Domains.Create(RandomData.RandomString()).Result;
            Domain = Operations.Authority.Domains.All().First(d => d.Id == domainId);
        }

        public AuthorityContext Context { get; set; }
        public Domain Domain { get; set; }

        public void Dispose()
        {
            // in this case 'forceReload' needs to set to True as tests does not use the service interface
            foreach (Domain domain in Operations.Authority.Domains.All(true))
            {
                Operations.Authority.Domains.Delete(domain.Id).Wait();
            }
        }
    }
}
