using System.Threading.Tasks;
using Authority.DomainModel;
using Authority.IntegrationTests.Common;
using Authority.IntegrationTests.Fixtures;
using IdentityServer.UnitOfWork.Account;
using Xunit;

namespace Authority.IntegrationTests.Accounts
{
    public sealed class LoginTests : IClassFixture<AccountTestFixture>
    {
        private readonly AccountTestFixture _fixture;

        public LoginTests(AccountTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task UserLoginShouldSucceed()
        {
            string password = RandomData.RandomString(12, true);
            User user = await TestOperations.RegisterAndActivateUser(_fixture.Context, _fixture.Product.Id, password);

            UserLogIn loginOperation = new UserLogIn(_fixture.Context, _fixture.Product.Id, user.Email, password);
            bool result = await loginOperation.Do();

            Assert.True(result);
        }
    }
}
