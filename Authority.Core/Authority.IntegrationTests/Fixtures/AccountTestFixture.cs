using Authority.DomainModel;

namespace Authority.IntegrationTests.Fixtures
{
    public sealed class AccountTestFixture : FixtureBase
    {
        public override void Dispose()
        {
            foreach (Invite invite in Context.Invites)
            {
                Context.Invites.Remove(invite);
            }

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
