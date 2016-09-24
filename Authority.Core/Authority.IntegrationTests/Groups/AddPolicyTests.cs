using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Authority.DomainModel;
using Authority.IntegrationTests.Common;
using Authority.Groups;
using Xunit;

namespace Authority.IntegrationTests.Groups
{
    public sealed class AddPolicyTests
    {
        [Fact]
        public async Task AddPolicyToGroupShouldSucceed()
        {
            using (AuthorityTestContext testContext = new AuthorityTestContext())
            {
                // Arrange
                Group group = await TestOperations.CreateGroup(testContext.Context, testContext.Domain.Id, RandomData.RandomString());
                Policy policy = await TestOperations.CreatePolicy(testContext.Context, testContext.Domain.Id, RandomData.RandomString());

                // Act
                AddPolicyToGroup addToGroup = new AddPolicyToGroup(testContext.Context, group.Id, policy.Id);
                await addToGroup.Do();
                await addToGroup.CommitAsync();

                // Assert
                Guid groupId = group.Id;
                Guid policyId = policy.Id;

                group = await testContext.Context.Groups
                    .Include(g => g.Policies)
                    .FirstOrDefaultAsync(g => g.Id == groupId);

                policy = await testContext.Context.Policies
                    .Include(p => p.Groups)
                    .FirstOrDefaultAsync(p => p.Id == policyId);

                Assert.NotNull(group);
                Assert.NotNull(policy);

                Assert.Equal(1, group.Policies.Count);
                Assert.Equal(1, policy.Groups.Count);

                Assert.True(group.Policies.First().Id == policyId);
                Assert.True(policy.Groups.First().Id == groupId);
            }
        }
    }
}
