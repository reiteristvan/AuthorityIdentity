using Authority.DomainModel;

namespace Authority.IntegrationTests.Fixtures
{
    public sealed class PolicyTestFixture : FixtureBase
    {
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
