using System;
using System.Data.Entity;
using System.Threading.Tasks;
using AuthorityIdentity.Groups;
using AuthorityIdentity.DomainModel;
using AuthorityIdentity.IntegrationTests.Common;
using Xunit;

namespace AuthorityIdentity.IntegrationTests.Groups
{
    public sealed class RemovePolicyTests
    {
        [Fact]
        public async Task RemovePolicyFromGroupShouldSucceed()
        {
            using (AuthorityTestContext testContext = new AuthorityTestContext())
            {
                // Arrange
                Group group = await TestOperations.CreateGroup(testContext.Context, testContext.Domain.Id, RandomData.RandomString());
                Policy policy = await TestOperations.CreatePolicy(testContext.Context, testContext.Domain.Id, RandomData.RandomString());

                AddPolicyToGroup addToGroup = new AddPolicyToGroup(testContext.Context, group.Id, policy.Id);
                await addToGroup.Do();
                await addToGroup.CommitAsync();

                // Act
                RemovePolicyFromGroup removeFromGroup = new RemovePolicyFromGroup(testContext.Context, group.Id, policy.Id);
                await removeFromGroup.Do();
                await removeFromGroup.CommitAsync();

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

                Assert.Equal(0, group.Policies.Count);
                Assert.Equal(0, policy.Groups.Count);
            }
        }
    }
}
