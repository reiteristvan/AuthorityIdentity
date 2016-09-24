using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Authority.DomainModel;
using Authority.IntegrationTests.Common;
using Authority.Groups;
using Xunit;

namespace Authority.IntegrationTests.Groups
{
    public sealed class CreateTests
    {
        [Fact]
        public async Task CreateGroupShouldSucceed()
        {
            using (AuthorityTestContext testContext = new AuthorityTestContext())
            {
                // Arrange
                string groupName = RandomData.RandomString();

                // Act
                CreateGroup create = new CreateGroup(testContext.Context, testContext.Domain.Id, groupName);
                Group group = await create.Do();
                await create.CommitAsync();

                // Assert
                group = testContext.Context.ReloadEntity<Group>(group.Id);

                Assert.NotNull(group);
                Assert.Equal(groupName, group.Name);
            }
        }

        [Fact]
        public async Task CreateGroupWithUsersShouldSucceed()
        {
            using (AuthorityTestContext testContext = new AuthorityTestContext())
            {
                // Arrange
                string groupName = RandomData.RandomString();
                const int numberOfUsers = 15;
                List<User> users = await TestOperations.CreateUsers(testContext.Context, testContext.Domain.Id, numberOfUsers);

                // Act
                CreateGroup create = new CreateGroup(testContext.Context, testContext.Domain.Id, groupName, false, false, users);
                Group group = await create.Do();
                await create.CommitAsync();

                // Assert
                group = testContext.Context.ReloadEntity<Group>(group.Id);

                Assert.NotNull(group);
                Assert.Equal(groupName, group.Name);

                users = testContext.Context.Users
                    .Include(u => u.Groups)
                    .Where(u => u.DomainId == testContext.Domain.Id)
                    .ToList();

                Assert.NotNull(users);
                Assert.Equal(numberOfUsers, users.Count);
                Assert.True(users.All(u => u.Groups.Count == 1 && u.Groups.First().Name == groupName));
            }
        }
    }
}
