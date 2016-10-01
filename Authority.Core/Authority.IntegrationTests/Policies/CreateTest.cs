using System.Threading.Tasks;
using AuthorityIdentity.Policies;
using AuthorityIdentity.DomainModel;
using AuthorityIdentity.IntegrationTests.Common;
using Xunit;

namespace AuthorityIdentity.IntegrationTests.Policies
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
