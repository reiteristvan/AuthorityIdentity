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
    public sealed class AddUsersToGroupTests
    {
        [Fact]
        public async Task AddUsersToGroupShouldSucceed()
        {
            using (AuthorityTestContext testContext = new AuthorityTestContext())
            {
                // Arrange
                const int numberOfUsers = 10;
                Group group = await TestOperations.CreateGroup(testContext.Context, testContext.Domain.Id, RandomData.RandomString());
                List<User> users = await TestOperations.CreateUsers(testContext.Context, testContext.Domain.Id, numberOfUsers);

                // Act
                AddUsersToGroup addToGroup = new AddUsersToGroup(testContext.Context, group.Id, users.Select(u => u.Id));
                await addToGroup.Do();
                await addToGroup.CommitAsync();

                // Assert
                Guid groupId = group.Id;
                IEnumerable<Guid> userIds = users.Select(u => u.Id);

                group = await testContext.Context.Groups
                    .Include(g => g.Users)
                    .FirstOrDefaultAsync(g => g.Id == groupId);

                users = testContext.Context.Users
                    .Include(u => u.Groups)
                    .Where(u => userIds.Contains(u.Id)).ToList();

                Assert.NotNull(group);
                Assert.NotNull(users);
                Assert.Equal(numberOfUsers, users.Count);

                Assert.Equal(numberOfUsers, group.Users.Count);
                Assert.True(users.All(u => u.Groups.Count == 1));
            }
        }
    }
}
