using System;
using System.Threading.Tasks;
using AuthorityIdentity.DomainModel;
using AuthorityIdentity.IntegrationTests.Common;
using AuthorityIdentity.Account;
using Xunit;

namespace AuthorityIdentity.IntegrationTests.Accounts
{
    public sealed class UserStatusTests
    {
        [Fact]
        public async Task SetStatusShouldSucceed()
        {
            using (AuthorityTestContext testContext = new AuthorityTestContext())
            {
                User user = await TestOperations.RegisterAndActivateUser(
                    testContext.Context, 
                    testContext.Domain.Id,
                    RandomData.RandomString());

                const bool isActive = false;
                SetUserStatus operation = new SetUserStatus(testContext.Context, user.Id, isActive);
                await operation.Do();
                await operation.CommitAsync();

                user = testContext.Context.ReloadEntity<User>(user.Id);

                Assert.Equal(isActive, user.IsActive);
            }
        }

        [Fact]
        public async Task SetStatusUserNotExistsShouldFail()
        {
            using (AuthorityTestContext testContext = new AuthorityTestContext())
            {
                await AssertExtensions.ThrowAsync<RequirementFailedException>(async () =>
                {
                    SetUserStatus operation = new SetUserStatus(testContext.Context, Guid.NewGuid(), true);
                    await operation.Do();
                    await operation.CommitAsync();
                },
                exception => exception.ErrorCode == ErrorCodes.UserNotFound);
            }
        }
    }
}
