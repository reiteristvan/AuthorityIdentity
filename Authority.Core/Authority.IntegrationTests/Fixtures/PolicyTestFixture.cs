using Authority.DomainModel;
using Authority.IntegrationTests.Common;

namespace Authority.IntegrationTests.Fixtures
{
    public sealed class PolicyTestFixture : FixtureBase
    {
        public PolicyTestFixture()
        {
            Developer = TestOperations.RegisterAndActivateDeveloper(Context, RandomData.RandomString(12, true)).Result;
            Product = TestOperations.CreateProductAndPublish(Context, Developer.Id).Result;
        }

        public Product Product { get; private set; }
        public Developer Developer { get; private set; }

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

            foreach (Developer developer in Context.Developers)
            {
                Context.Developers.Remove(developer);
            }

            Context.SaveChanges();
            Context.Dispose();
        }
    }
}
