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
    public sealed class DeleteTests
    {
        [Fact]
        public async Task DeleteGroupShouldSucceed()
        {
            using (AuthorityTestContext testContext = new AuthorityTestContext())
            {
                // Arrange
                Group group = await TestOperations.CreateGroup(testContext.Context, testContext.Domain.Id, RandomData.RandomString());

                // Act
                DeleteGroup delete = new DeleteGroup(testContext.Context, group.Id);
                await delete.Do();
                await delete.CommitAsync();

                // Assert
                group = testContext.Context.ReloadEntity<Group>(group.Id);
                Assert.Null(group);
            }
        }

        [Fact]
        public async Task DeleteGroupWithUsersShouldSucceed()
        {
            using (AuthorityTestContext testContext = new AuthorityTestContext())
            {
                // Arrange
                const int numberOfUsers = 10;
                Group group = await TestOperations.CreateGroup(testContext.Context, testContext.Domain.Id, RandomData.RandomString());
                List<User> users = await TestOperations.CreateUsers(testContext.Context, testContext.Domain.Id, numberOfUsers);

                AddUsersToGroup addToGroup = new AddUsersToGroup(testContext.Context, group.Id, users.Select(u => u.Id));
                await addToGroup.Do();
                await addToGroup.CommitAsync();

                // Act
                DeleteGroup delete = new DeleteGroup(testContext.Context, group.Id);
                await delete.Do();
                await delete.CommitAsync();

                // Assert
                group = testContext.Context.ReloadEntity<Group>(group.Id);
                Assert.Null(group);

                foreach (User user in users)
                {
                    User temp = await testContext.Context.Users
                        .Include(u => u.Groups)
                        .FirstOrDefaultAsync(u => u.Id == user.Id);

                    Assert.NotNull(temp);
                    Assert.Equal(0, temp.Groups.Count);
                }
            }
        }
    }
}
