using System.Threading.Tasks;
using AuthorityIdentity.Account;
using AuthorityIdentity.DomainModel;
using AuthorityIdentity.IntegrationTests;
using AuthorityIdentity.IntegrationTests.Common;
using Xunit;

namespace Authority.IntegrationTests.Accounts
{
    public sealed class ExternalUserTests
    {
        [Fact]
        public async Task LoginWithExternalUserShouldSucceed()
        {
            using (AuthorityTestContext testContext = new AuthorityTestContext())
            {
                // Arrange

                // Act
                LoginWithExternalUser login = new LoginWithExternalUser(testContext.Context, testContext.Domain.Id, TestExternalProvider.ProviderName, "");
                User user = await login.Do();
                await login.CommitAsync();

                // Assert
                user = testContext.Context.ReloadEntity<User>(user.Id);

                Assert.True(user.IsExternal);
                Assert.Equal(TestExternalProvider.LastUser.Email, user.Email);
            }
        }

        [Fact]
        public async Task ReLoginWithExternalUserShouldSucceed()
        {
            using (AuthorityTestContext testContext = new AuthorityTestContext())
            {
                // Arrange
                LoginWithExternalUser login = new LoginWithExternalUser(testContext.Context, testContext.Domain.Id, TestExternalProvider.ProviderName, "");
                User user = await login.Do();
                await login.CommitAsync();

                // Act
                LoginWithExternalUser reLogin = new LoginWithExternalUser(testContext.Context, testContext.Domain.Id, TestExternalProvider.ProviderName, "");
                User reUser = await reLogin.Do();
                await reLogin.CommitAsync();

                // Assert
                user = testContext.Context.ReloadEntity<User>(reUser.Id);

                Assert.True(reUser.IsExternal);
                Assert.Equal(TestExternalProvider.LastUser.Email, reUser.Email);
                Assert.Equal(user.Id, reUser.Id);
            }
        }
    }
}
