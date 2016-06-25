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
            foreach (AuthorityClaim authorityClaim in Context.Claims)
            {
                Context.Claims.Remove(authorityClaim);
            }

            //Context.SaveChanges();

            foreach (Policy policy in Context.Policies)
            {
                Context.Policies.Remove(policy);
            }

            //Context.SaveChanges();

            foreach (Domain domain in Context.Domains)
            {
                Context.Domains.Remove(domain);
            }

            Context.SaveChanges();
            Context.Dispose();
        }
    }
}
