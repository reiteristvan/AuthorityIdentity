using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Authority.DomainModel;
using Authority.IntegrationTests.Common;
using Authority.Operations.Groups;
using Xunit;

namespace Authority.IntegrationTests.Groups
{
    public sealed class RemoveUsersFromGroupTests
    {
        [Fact]
        public async Task RemoveUsersFromGroupShouldSucceed()
        {
            using (AuthorityTestContext testContext = new AuthorityTestContext())
            {
                // Arrange
                const int numberOfUsers = 10;
                List<User> users = await TestOperations.CreateUsers(testContext.Context, testContext.Domain.Id, numberOfUsers);
                Group group = await TestOperations.CreateGroup(testContext.Context, testContext.Domain.Id, RandomData.RandomString());

                AddUsersToGroup addToGroup = new AddUsersToGroup(testContext.Context, group.Id, users.Select(u => u.Id));
                await addToGroup.Do();
                await addToGroup.CommitAsync();

                // Act
                const int numberOfUsersToRemove = 5;
                IEnumerable<Guid> usersToRemove = users.Take(numberOfUsersToRemove).Select(u => u.Id);
                RemoveUsersFromGroup removeFromGroup = new RemoveUsersFromGroup(testContext.Context, group.Id, usersToRemove);
                await removeFromGroup.Do();
                await removeFromGroup.CommitAsync();

                // Assert
                List<User> removedUsers = testContext.Context.Users
                    .Include(u => u.Groups)
                    .Where(u => usersToRemove.Contains(u.Id))
                    .ToList();

                Guid groupId = group.Id;
                group = await testContext.Context.Groups
                    .Include(g => g.Users)
                    .FirstOrDefaultAsync(g => g.Id == groupId);

                Assert.True(removedUsers.All(u => u.Groups.Count == 0));
                Assert.Equal(numberOfUsers - numberOfUsersToRemove, group.Users.Count);
            }
        }
    }
}
