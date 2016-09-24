using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Authority.DomainModel;
using Authority.IntegrationTests.Common;
using Authority.Policies;
using Xunit;

namespace Authority.IntegrationTests.Policies
{
    public sealed class RemoveUserFromPolicyTests
    {
        [Fact]
        public async Task RemoveUserFromPolicyShouldSucceed()
        {
            using (AuthorityTestContext testContext = new AuthorityTestContext())
            {
                // Arrange
                User user = await TestOperations.RegisterAndActivateUser(testContext.Context, testContext.Domain.Id, RandomData.RandomString());
                Policy policy = await TestOperations.CreatePolicy(testContext.Context, testContext.Domain.Id, RandomData.RandomString());
                
                AddUserToPolicy addToPolicy = new AddUserToPolicy(testContext.Context, user.Id, policy.Id);
                await addToPolicy.Do();
                await addToPolicy.CommitAsync();

                // Act
                RemoveUserFromPolicy removeFromPolicy = new RemoveUserFromPolicy(testContext.Context, user.Id, policy.Id);
                await removeFromPolicy.Do();
                await removeFromPolicy.CommitAsync();

                // Assert
                Guid userId = user.Id;
                user = await testContext.Context.Users
                    .Include(u => u.Policies)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                Assert.Empty(user.Policies);
            }
        }
    }
}
