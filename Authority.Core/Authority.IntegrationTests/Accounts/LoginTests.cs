using System.Threading.Tasks;
using AuthorityIdentity.DomainModel;
using AuthorityIdentity.IntegrationTests.Common;
using AuthorityIdentity.Account;
using Xunit;

namespace AuthorityIdentity.IntegrationTests.Accounts
{
    public sealed class LoginTests
    {
        [Fact]
        public async Task UserLoginShouldSucceed()
        {
            using (AuthorityTestContext testContext = new AuthorityTestContext())
            {
                string password = RandomData.RandomString(12, true);
                User user = await TestOperations.RegisterAndActivateUser(testContext.Context, testContext.Domain.Id, password);

                LoginUser loginOperation = new LoginUser(testContext.Context, testContext.Domain.Id, user.Email, password);
                LoginResult result = await loginOperation.Do();
                await loginOperation.CommitAsync();

                Assert.True(!string.IsNullOrEmpty(result.Email));
            }
        }
    }
}
