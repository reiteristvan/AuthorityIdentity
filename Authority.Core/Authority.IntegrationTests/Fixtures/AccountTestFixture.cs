using Authority.DomainModel;
using Authority.IntegrationTests.Common;

namespace Authority.IntegrationTests.Fixtures
{
    public sealed class AccountTestFixture : FixtureBase
    {
        public AccountTestFixture()
        {
            Operations.Authority.Init();
            Domain = TestOperations.CreateDomain(Context).Result;
        }

        public Domain Domain { get; private set; }

        public override void Dispose()
        {
            foreach (User user in Context.Users)
            {
                Context.Users.Remove(user);
            }

            Context.SaveChanges();

            foreach (Domain domain in Context.Domains)
            {
                Context.Domains.Remove(domain);
            }

            Context.SaveChanges();
            Context.Dispose();
        }
    }
}
