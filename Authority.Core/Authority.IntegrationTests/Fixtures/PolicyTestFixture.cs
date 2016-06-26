using Authority.DomainModel;
using Authority.IntegrationTests.Common;

namespace Authority.IntegrationTests.Fixtures
{
    public sealed class PolicyTestFixture : FixtureBase
    {
        public PolicyTestFixture()
        {
            Domain = TestOperations.CreateDomain(Context).Result;
        }

        public Domain Domain { get; private set; }

        public override void Dispose()
        {
            foreach (Domain domain in Context.Domains)
            {
                Context.Domains.Remove(domain);
            }

            Context.SaveChanges();
            Context.Dispose();
        }
    }
}
