using System;
using System.Linq;
using Authority.DomainModel;
using Authority.EntityFramework;

namespace Authority.IntegrationTests.Fixtures
{
    public abstract class FixtureBase : IDisposable
    {
        protected FixtureBase()
        {
            Operations.Authority.Init();
            Context = new AuthorityContext();
            Domain = Operations.Authority.Domains.All().First();
        }

        public Domain Domain { get; private set; }
        public AuthorityContext Context { get; private set; }

        /// <summary>
        /// This needs to be overrided in different implementations based on the entities the test uses
        /// </summary>
        public abstract void Dispose();
    }
}
