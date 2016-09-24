using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Authority.DomainModel;
using Authority.IntegrationTests.Common;
using Authority.Policies;
using Xunit;

namespace Authority.IntegrationTests.Policies
{
    public sealed class AddUserToPolicyTests
    {
        [Fact]
        public async Task AddUserToPolicyShouldSucceed()
        {
            using (AuthorityTestContext testContext = new AuthorityTestContext())
            {
                // Arrange
                User user = await TestOperations.RegisterAndActivateUser(testContext.Context, testContext.Domain.Id, RandomData.RandomString());
                Policy policy = await TestOperations.CreatePolicy(testContext.Context, testContext.Domain.Id, RandomData.RandomString(), false);

                // Act
                AddUserToPolicy addToPolicy = new AddUserToPolicy(testContext.Context, user.Id, policy.Id);
                await addToPolicy.Do();
                await addToPolicy.CommitAsync();

                // Assert
                Guid userId = user.Id;
                user = await testContext.Context.Users
                    .Include(u => u.Policies)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                Assert.NotEmpty(user.Policies);
                Assert.True(user.Policies.First().Name == policy.Name);
            }
        }
    }
}
