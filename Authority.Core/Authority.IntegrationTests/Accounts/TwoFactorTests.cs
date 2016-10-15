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
    }
}
