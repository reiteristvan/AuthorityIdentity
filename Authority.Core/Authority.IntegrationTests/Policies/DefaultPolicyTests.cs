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
    public sealed class DefaultPolicyTests
    {
        [Fact]
        public async Task RegisterUserWithDefaultPolicyShouldSucceed()
        {
            using (AuthorityTestContext testContext = new AuthorityTestContext())
            {
                Policy defaultPolicy = await TestOperations.CreatePolicy(
                    testContext.Context,
                    testContext.Domain.Id,
                    RandomData.RandomString(),
                    true);

                User user = await TestOperations.RegisterAndActivateUser(
                    testContext.Context,
                    testContext.Domain.Id,
                    RandomData.RandomString());

                Guid domainId = testContext.Domain.Id;
                string email = user.Email;

                user = await testContext.Context.Users
                    .Include(u => u.Policies)
                    .FirstAsync(u => u.DomainId == domainId && u.Email == email);

                Assert.NotNull(user.Policies);
                Assert.True(user.Policies.Any(p => p.Id == defaultPolicy.Id));
            }
        }

        [Fact]
        public async Task CreateDefaultPolicyWithExistingDefaultShouldSucceed()
        {
            using (AuthorityTestContext testContext = new AuthorityTestContext())
            {
                // Arrange
                string firstPolicyName = RandomData.RandomString();
                string secondPolicyName = RandomData.RandomString();

                CreatePolicy createFirstOperation = new CreatePolicy(testContext.Context, testContext.Domain.Id, firstPolicyName, true);
                Policy first = await createFirstOperation.Do();
                await createFirstOperation.CommitAsync();

                // Act
                CreatePolicy createSecondOperation = new CreatePolicy(testContext.Context, testContext.Domain.Id, secondPolicyName, true, true);
                Policy second = await createSecondOperation.Do();
                await createSecondOperation.CommitAsync();

                // Assert
                first = testContext.Context.ReloadEntity<Policy>(first.Id);

                Assert.True(second.Default);
                Assert.False(first.Default);
            }
        }
    }
}
