using System.Threading.Tasks;
using Authority.DomainModel;
using Authority.IntegrationTests.Common;
using Authority.IntegrationTests.Fixtures;
using Authority.Operations;
using Authority.Operations.Account;
using Xunit;

namespace Authority.IntegrationTests.Accounts
{
    public sealed class UserStatusTests : IClassFixture<AccountTestFixture>
    {
        private readonly AccountTestFixture _fixture;

        public UserStatusTests(AccountTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task SetStatusShouldSucceed()
        {
            User user = await TestOperations.RegisterAndActivateUser(_fixture.Context, _fixture.Domain.Id, RandomData.RandomString());

            const bool isActive = false;
            SetUserStatus operation = new SetUserStatus(_fixture.Context, _fixture.Domain.Id, user.Email, isActive);
            await operation.Do();
            await operation.CommitAsync();

            user = _fixture.Context.ReloadEntity<User>(user.Id); // it's a cheat, Id is not the PK

            Assert.Equal(isActive, user.IsActive);
        }

        [Fact]
        public async Task SetStatusUserNotExistsShouldFail()
        {
            await AssertExtensions.ThrowAsync<RequirementFailedException>(async () =>
            {
                SetUserStatus operation = new SetUserStatus(_fixture.Context, _fixture.Domain.Id, RandomData.Email(), true);
                await operation.Do();
                await operation.CommitAsync();
            },
            exception => exception.ErrorCode == AccountErrorCodes.UserNotFound);
        }
    }
}
