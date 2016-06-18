using Authority.DomainModel;
using Authority.IntegrationTests.Common;

namespace Authority.IntegrationTests.Fixtures
{
    public sealed class PolicyTestFixture : FixtureBase
    {
        public PolicyTestFixture()
        {
            Product = TestOperations.CreateProductAndPublish(Context).Result;
        }

        public Product Product { get; private set; }

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

            foreach (Product product in Context.Products)
            {
                Context.Products.Remove(product);
            }

            Context.SaveChanges();
            Context.Dispose();
        }
    }
}
