using System.Threading.Tasks;
using AuthorityIdentity.Account;
using AuthorityIdentity.DomainModel;
using AuthorityIdentity.IntegrationTests;
using AuthorityIdentity.IntegrationTests.Common;
using Xunit;

namespace Authority.IntegrationTests.Accounts
{
    public sealed class TwoFactorTests
    {
        [Fact]
        public async Task AddTwoFactorToUserShouldSucceed()
        {
            using (AuthorityTestContext testContext = new AuthorityTestContext())
            {
                // Arrange
                User user = await TestOperations.RegisterUser(testContext.Context, testContext.Domain.Id, RandomData.RandomString());

                // Act
                AddTwoFactorAuthenticationModel model = new AddTwoFactorAuthenticationModel
                {
                    TwoFactorType = TwoFactorType.Email,
                    TwoFactorTarget = "test@test.com",
                    UserId = user.Id
                };
                AddTwoFactorAuthenticationToUser operation = new AddTwoFactorAuthenticationToUser(testContext.Context, model);
                await operation.Do();
                await operation.CommitAsync();

                // Assert
                user = testContext.Context.ReloadEntity<User>(user.Id);

                Assert.Equal(model.TwoFactorType, user.TwoFactorType);
                Assert.Equal(model.TwoFactorTarget, user.TwoFactorTarget);
                Assert.Equal(true, user.IsTwoFactorEnabled);
            }
        }

        [Fact]
        public async Task LoginWithTwoFactorShouldSucceed()
        {
            using (AuthorityTestContext testContext = new AuthorityTestContext())
            {
                // Arrange
                string password = RandomData.RandomString();
                string email;

                User user = await TestOperations.RegisterAndActivateUser(testContext.Context, testContext.Domain.Id, password);
                email = user.Email;

                AddTwoFactorAuthenticationModel model = new AddTwoFactorAuthenticationModel
                {
                    TwoFactorTarget = RandomData.RandomString(),
                    TwoFactorType = TwoFactorType.Other,
                    UserId = user.Id
                };
                AddTwoFactorAuthenticationToUser addTwoFactor = new AddTwoFactorAuthenticationToUser(testContext.Context, model);
                await addTwoFactor.Do();
                await addTwoFactor.CommitAsync();

                // Act
                LoginUser login = new LoginUser(testContext.Context, testContext.Domain.Id, email, password);
                LoginResult loginResult = await login.Do();
                await login.CommitAsync();

                Assert.True(loginResult.WaitForTwoFactor);

                // Assert
                FinalizeTwoFactorAuthentication finalize = new FinalizeTwoFactorAuthentication(testContext.Context, user.Id, TestTwoFactorService.LastToken);
                bool result = await finalize.Do();
                await finalize.CommitAsync();

                Assert.True(result);

                user = testContext.Context.ReloadEntity<User>(user.Id);

                Assert.Equal("", user.TwoFactorToken);
            }
        }
    }
}
