using System.Threading.Tasks;
using AuthorityIdentity.Account;
using AuthorityIdentity.DomainModel;
using AuthorityIdentity.IntegrationTests;
using AuthorityIdentity.IntegrationTests.Common;
using Xunit;

namespace Authority.IntegrationTests.Accounts
{
    public sealed class MetadataTests
    {
        [Fact]
        public async Task SetMetadataShouldSucceed()
        {
            using (AuthorityTestContext testContext = new AuthorityTestContext())
            {
                // Arrange
                User user = await TestOperations.RegisterAndActivateUser(
                    testContext.Context,
                    testContext.Domain.Id,
                    RandomData.RandomString());

                // Act
                string metadata = RandomData.RandomString(512);
                SetMetadata setMetadata = new SetMetadata(testContext.Context, user.Id, metadata);
                await setMetadata.Do();
                await setMetadata.CommitAsync();

                // Assert
                user = testContext.Context.ReloadEntity<User>(user.Id);

                Assert.NotNull(user);
                Assert.Equal(metadata, user.Metadata);
            }
        }
    }
}
