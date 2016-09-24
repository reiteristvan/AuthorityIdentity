using System.Threading.Tasks;
using Authority.DomainModel;
using Authority.IntegrationTests.Common;
using Authority.Policies;
using Xunit;

namespace Authority.IntegrationTests.Policies
{
    public sealed class CreateTest
    {
        [Fact]
        public async Task CreatePolicyShouldSucceed()
        {
            using (AuthorityTestContext testContext = new AuthorityTestContext())
            {
                CreatePolicy create = new CreatePolicy(
                    testContext.Context,
                    testContext.Domain.Id,
                    RandomData.RandomString(),
                    true);

                Policy policy = await create.Do();
                await create.CommitAsync();

                policy = testContext.Context.ReloadEntity<Policy>(policy.Id);

                Assert.NotNull(policy);
            }
        }
    }
}
