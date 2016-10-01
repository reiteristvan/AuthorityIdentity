using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AuthorityIdentity.Groups;
using AuthorityIdentity.DomainModel;
using AuthorityIdentity.IntegrationTests.Common;
using Xunit;

namespace AuthorityIdentity.IntegrationTests.Groups
{
    public sealed class DefaultGroupTests
    {
        [Fact]
        public async Task RegisterUserWithDefaultGroupShouldSucceed()
        {
            using (AuthorityTestContext testContext = new AuthorityTestContext())
            {
                // Arrange
                string groupName = RandomData.RandomString();

                CreateGroup create = new CreateGroup(testContext.Context, testContext.Domain.Id, groupName, defaultGroup: true);
                Group group = await create.Do();
                await create.CommitAsync();

                // Act
                User user = await TestOperations.RegisterAndActivateUser(testContext.Context, testContext.Domain.Id, RandomData.RandomString());

                // Assert
                Guid userId = user.Id;
                user = await testContext.Context.Users
                    .Include(u => u.Groups)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                Assert.NotNull(user);
                Assert.Equal(1, user.Groups.Count);
                Assert.True(user.Groups.First().Id == group.Id && user.Groups.First().Name == groupName);
            }
        }
    }
}
