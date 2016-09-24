using System.Threading.Tasks;
using Authority.IntegrationTests.Common;
using Authority.Account;
using Xunit;

namespace Authority.IntegrationTests.Accounts
{
    public sealed class RegistrationTests
    {
        [Fact]
        public async Task RegistrationShouldSucceed()
        {
            using (AuthorityTestContext testContext = new AuthorityTestContext())
            {
                string email = RandomData.Email();
                string username = RandomData.RandomString();
                string password = RandomData.RandomString(12, true);

                RegisterUser operation = new RegisterUser(testContext.Context, testContext.Domain.Id, email, username,
                    password);
                await operation.Do();
                await operation.CommitAsync();
            }
        }

        [Fact]
        public async Task RegistrationDuplicateUserShouldFail()
        {
            using (AuthorityTestContext testContext = new AuthorityTestContext())
            {
                string email = RandomData.Email();
                string username = RandomData.RandomString();
                string password = RandomData.RandomString(12, true);

                RegisterUser first = new RegisterUser(testContext.Context, testContext.Domain.Id, email, username, password);
                await first.Do();

                await first.CommitAsync();

                await AssertExtensions.ThrowAsync<RequirementFailedException>(async () =>
                {
                    RegisterUser second = new RegisterUser(testContext.Context, testContext.Domain.Id, email, username, password);
                    await second.Do();
                },
                exception => exception.ErrorCode == ErrorCodes.EmailAlreadyExists);
            }
        }

        [Fact]
        public async Task RegistrationUsernameExistsShouldFail()
        {
            using (AuthorityTestContext testContext = new AuthorityTestContext())
            {
                string email = RandomData.Email();
                string username = RandomData.RandomString();
                string password = RandomData.RandomString(12, true);

                RegisterUser first = new RegisterUser(testContext.Context, testContext.Domain.Id, email, username, password);
                await first.Do();
                await first.CommitAsync();

                await AssertExtensions.ThrowAsync<RequirementFailedException>(async () =>
                {
                    string userEmail = RandomData.Email();
                    RegisterUser second = new RegisterUser(testContext.Context, testContext.Domain.Id, userEmail, username, password);
                    await second.Do();
                },
                exception => exception.ErrorCode == ErrorCodes.UsernameNotAvailable);
            }
        }
    }
}
