using Authority.DomainModel;

namespace Authority.IntegrationTests.Fixtures
{
    public sealed class AccountTestFixture : FixtureBase
    {
        public override void Dispose()
        {
            foreach (User user in Context.Users)
            {
                Context.Users.Remove(user);
            }

            Context.SaveChanges();

            foreach (Product product in Context.Products)
            {
                Context.Products.Remove(product);
            }

            Context.SaveChanges();

            Context.Dispose();
        }
    }
}
