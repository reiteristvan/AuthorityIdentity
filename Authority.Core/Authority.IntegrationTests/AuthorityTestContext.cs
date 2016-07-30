using System;
using System.Linq;
using Authority.DomainModel;
using Authority.EntityFramework;
using Authority.Operations.Configuration;

namespace Authority.IntegrationTests
{
    public sealed class AuthorityTestContext : IDisposable
    {
        static AuthorityTestContext()
        {
            Operations.Authority.Configuration = new AuthorityConfiguration
            {
                DomainMode = DomainMode.Multi
            };

            Operations.Authority.Logger = new TestLogger();
            Operations.Authority.Init();
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
            Operations.Authority.Domains.Delete(Domain.Id);
        }
    }
}
