using System;
using Authority.DomainModel;
using Authority.IntegrationTests.Common;

namespace Authority.IntegrationTests.Fixtures
{
    public sealed class AccountTestFixture : FixtureBase
    {
        public AccountTestFixture()
        {
            Product = TestOperations.CreateProductAndPublish(Context).Result;
        }

        public Product Product { get; private set; }

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
