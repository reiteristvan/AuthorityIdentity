using System.Threading.Tasks;
using AuthorityIdentity.DomainModel;
using AuthorityIdentity.IntegrationTests.Common;
using AuthorityIdentity.Claims;
using Xunit;

namespace AuthorityIdentity.IntegrationTests.Claims
{
    public sealed class UpdateTests
    {
        [Fact]
        public async Task UpdateClaimShouldSucceed()
        {
            using (AuthorityTestContext testContext = new AuthorityTestContext())
            {
                // Arrange
                string friendlyName = RandomData.RandomString();
                string issuer = RandomData.RandomString();
                string type = RandomData.RandomString();
                string value = RandomData.RandomString();

                AuthorityClaim claim = await TestOperations.CreateClaim(testContext.Context, testContext.Domain.Id, friendlyName, issuer, type, value);    

                // Act
                string newName = RandomData.RandomString();
                UpdateClaim update = new UpdateClaim(testContext.Context, claim.Id, newName, issuer, type, value);
                await update.Do();
                await update.CommitAsync();

                // Assert
                claim = testContext.Context.ReloadEntity<AuthorityClaim>(claim.Id);
                Assert.Equal(newName, claim.FriendlyName);
            }
        }
    }
}
