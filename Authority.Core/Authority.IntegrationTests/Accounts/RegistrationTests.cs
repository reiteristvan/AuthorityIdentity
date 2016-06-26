using System.Threading.Tasks;
using Authority.IntegrationTests;
using Authority.IntegrationTests.Common;
using Authority.IntegrationTests.Fixtures;
using Authority.Operations;
using Authority.Operations.Account;
using Xunit;

namespace IdentityServer.IntegrationTests.Accounts
{
    public sealed class RegistrationTests : IClassFixture<AccountTestFixture>
    {
        private readonly AccountTestFixture _fixture;

        public RegistrationTests(AccountTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task RegistrationShouldSucceed()
        {
            string email = RandomData.Email();
            string username = RandomData.RandomString();
            string password = RandomData.RandomString(12, true);

            RegisterUser operation = new RegisterUser(_fixture.Context, _fixture.Domain.Id, email, username, password);
            await operation.Do();
            await operation.CommitAsync();
        }

        [Fact]
        public async Task RegistrationDuplicateUserShouldFail()
        {
            string email = RandomData.Email();
            string username = RandomData.RandomString();
            string password = RandomData.RandomString(12, true);

            RegisterUser first = new RegisterUser(_fixture.Context, _fixture.Domain.Id, email, username, password);
            await first.Do();

            await first.CommitAsync();

            await AssertExtensions.ThrowAsync<RequirementFailedException>(async () =>
            {
                RegisterUser second = new RegisterUser(_fixture.Context, _fixture.Domain.Id, email, username, password);
                await second.Do();
            },
            exception => exception.ErrorCode == AccountErrorCodes.EmailAlreadyExists);
        }

        [Fact]
        public async Task RegistrationUsernameExistsShouldFail()
        {
            string email = RandomData.Email();
            string username = RandomData.RandomString();
            string password = RandomData.RandomString(12, true);

            RegisterUser first = new RegisterUser(_fixture.Context, _fixture.Domain.Id, email, username, password);
            await first.Do();

            await first.CommitAsync();

            await AssertExtensions.ThrowAsync<RequirementFailedException>(async () =>
            {
                string userEmail = RandomData.Email();
                RegisterUser second = new RegisterUser(_fixture.Context, _fixture.Domain.Id, userEmail, username, password);
                await second.Do();
            },
            exception => exception.ErrorCode == AccountErrorCodes.UsernameNotAvailable);
        }
    }
}
