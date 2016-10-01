using System.Collections.Generic;
using System.Threading.Tasks;
using AuthorityIdentity.Policies;
using AuthorityIdentity.DomainModel;
using AuthorityIdentity.IntegrationTests.Common;
using Xunit;

namespace AuthorityIdentity.IntegrationTests.Policies
{
    public sealed class DeleteTests
    {
        [Fact]
        public async Task DeletePolicyShouldSucceed()
        {
            using (AuthorityTestContext testContext = new AuthorityTestContext())
            {
                // Arrange
                CreatePolicy create = new CreatePolicy(testContext.Context, testContext.Domain.Id, RandomData.RandomString());
                Policy policy = await create.Do();
                await create.CommitAsync();

                // Act
                DeletePolicy delete = new DeletePolicy(testContext.Context, policy.Id);
                await delete.Do();
                await delete.CommitAsync();

                // Assert
                policy = testContext.Context.ReloadEntity<Policy>(policy.Id);

                Assert.Null(policy);
            }
        }

        [Fact]
        public async Task AddUserToPolicyThenDeleteUserShouldRemain()
        {
            using (AuthorityTestContext testContext = new AuthorityTestContext())
            {
                // Arrange
                Policy policy = await TestOperations.CreatePolicy(
                    testContext.Context, 
                    testContext.Domain.Id, 
                    RandomData.RandomString(),
                    true);

                List<User> users = await TestOperations.CreateUsers(testContext.Context, testContext.Domain.Id);

                // Act
                DeletePolicy delete = new DeletePolicy(testContext.Context, policy.Id);
                await delete.Do();
                await delete.CommitAsync();

                // Assert
                policy = testContext.Context.ReloadEntity<Policy>(policy.Id);
                Assert.Null(policy);

                foreach (User user in users)
                {
                    User reloaded = testContext.Context.ReloadEntity<User>(user.Id);
                    Assert.NotNull(reloaded);
                }
            }
        }
    }
}
