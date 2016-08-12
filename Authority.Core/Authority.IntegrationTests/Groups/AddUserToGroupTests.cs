using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Authority.DomainModel;
using Authority.IntegrationTests.Common;
using Authority.Operations.Groups;
using Xunit;

namespace Authority.IntegrationTests.Groups
{
    public sealed class AddUserToGroupTests
    {
        [Fact]
        public async Task AddUserToGroupShouldSucceed()
        {
            using (AuthorityTestContext testContext = new AuthorityTestContext())
            {
                // Arrange
                Group group = await TestOperations.CreateGroup(testContext.Context, testContext.Domain.Id, RandomData.RandomString());
                User user = await TestOperations.RegisterAndActivateUser(testContext.Context, testContext.Domain.Id);

                // Act
                AddUserToGroup addToGroup = new AddUserToGroup(testContext.Context, user.Id, group.Id);
                await addToGroup.Do();
                await addToGroup.CommitAsync();

                // Assert
                Guid groupId = group.Id;
                Guid userId = user.Id;

                group = await testContext.Context.Groups
                    .Include(g => g.Users)
                    .FirstOrDefaultAsync(g => g.Id == groupId);

                user = await testContext.Context.Users
                    .Include(u => u.Groups)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                Assert.NotNull(group);
                Assert.NotNull(user);

                Assert.Equal(1, user.Groups.Count);
                Assert.Equal(1, group.Users.Count);

                Assert.True(user.Groups.First().Id == groupId);
                Assert.True(group.Users.First().Id == userId);
            }
        }
    }
}
