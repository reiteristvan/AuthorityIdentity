using System.Data.Entity;
using System.Threading.Tasks;
using AuthorityIdentity.IntegrationTests.Common;
using AuthorityIdentity.Account;
using AuthorityIdentity.DomainModel;
using Xunit;

namespace AuthorityIdentity.IntegrationTests.Accounts
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

                RegisterUserModel model = new RegisterUserModel
                {
                    DomainId = testContext.Domain.Id,
                    Email = email,
                    Username = username,
                    Password = password,
                    NeedToActivate = false
                };
                RegisterUser operation = new RegisterUser(testContext.Context, model);
                await operation.Do();
                await operation.CommitAsync();
            }
        }

        [Fact]
        public async Task RegistrationWithMetadataShouldSucceed()
        {
            using (AuthorityTestContext testContext = new AuthorityTestContext())
            {
                // Arrange
                string email = RandomData.Email();
                string username = RandomData.RandomString();
                string password = RandomData.RandomString(12, true);
                string metadata = RandomData.RandomString(256);

                // Act
                RegisterUserModel model = new RegisterUserModel
                {
                    DomainId = testContext.Domain.Id,
                    Email = email,
                    Username = username,
                    Password = password,
                    NeedToActivate = false,
                    Metadata = metadata
                };
                RegisterUser operation = new RegisterUser(testContext.Context, model);
                User user = await operation.Do();
                await operation.CommitAsync();

                //Assert
                user = testContext.Context.ReloadEntity<User>(user.Id);
                Metadata md = await testContext.Context.Metadata.FirstOrDefaultAsync(m => m.Id == user.Id);

                Assert.NotNull(user);
                Assert.NotNull(md);
                Assert.Equal(metadata, md.Data);
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

                RegisterUserModel model = new RegisterUserModel
                {
                    DomainId = testContext.Domain.Id,
                    Email = email,
                    Username = username,
                    Password = password,
                    NeedToActivate = false
                };
                RegisterUser first = new RegisterUser(testContext.Context, model);
                await first.Do();

                await first.CommitAsync();

                await AssertExtensions.ThrowAsync<RequirementFailedException>(async () =>
                {
                    RegisterUser second = new RegisterUser(testContext.Context, model);
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

                RegisterUserModel model = new RegisterUserModel
                {
                    DomainId = testContext.Domain.Id,
                    Email = email,
                    Username = username,
                    Password = password,
                    NeedToActivate = false
                };
                RegisterUser first = new RegisterUser(testContext.Context, model);
                await first.Do();
                await first.CommitAsync();

                await AssertExtensions.ThrowAsync<RequirementFailedException>(async () =>
                {
                    string userEmail = RandomData.Email();
                    model.Email = userEmail; // it should be immutable, but it is a test :)
                    RegisterUser second = new RegisterUser(testContext.Context, model);
                    await second.Do();
                },
                exception => exception.ErrorCode == ErrorCodes.UsernameNotAvailable);
            }
        }
    }
}
