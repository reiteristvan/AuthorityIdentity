using System.Threading.Tasks;
using Authority.DomainModel;
using Authority.IntegrationTests.Common;
using Authority.Account;
using Xunit;

namespace Authority.IntegrationTests.Accounts
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
