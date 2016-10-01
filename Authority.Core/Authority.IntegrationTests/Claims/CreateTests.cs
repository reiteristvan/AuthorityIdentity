using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AuthorityIdentity.DomainModel;
using AuthorityIdentity.EntityFramework;
using AuthorityIdentity.IntegrationTests.Common;
using AuthorityIdentity.Claims;
using Xunit;

namespace AuthorityIdentity.IntegrationTests.Claims
{
    public sealed class CreateTests
    {
        [Fact]
        public async Task CreateClaimShouldSucceed()
        {
            using (AuthorityTestContext testContext = new AuthorityTestContext())
            {
                // Arrange
                AuthorityContext context = testContext.Context;
                Guid domainId = testContext.Domain.Id;
                string friendlyName = RandomData.RandomString();
                string issuer = RandomData.RandomString();
                string type = RandomData.RandomString();
                string value = RandomData.RandomString();

                // Act
                CreateClaim create = new CreateClaim(context, domainId, friendlyName, issuer, type, value);
                AuthorityClaim claim = await create.Do();
                await create.CommitAsync();

                // Assert
                claim = context.ReloadEntity<AuthorityClaim>(claim.Id);
                Assert.NotNull(claim);

                Domain domainWithClaims = await context.Domains
                    .Include(d => d.Claims)
                    .FirstOrDefaultAsync(d => d.Id == domainId);

                Assert.True(domainWithClaims.Claims.Count == 1 && domainWithClaims.Claims.All(c => c.Id == claim.Id));
            }
        }

        [Fact]
        public async Task CreateClaimDuplicateNameShouldFail()
        {
            using (AuthorityTestContext testContext = new AuthorityTestContext())
            {
                // Arrange
                AuthorityContext context = testContext.Context;
                Guid domainId = testContext.Domain.Id;
                string friendlyName = RandomData.RandomString();
                string issuer = RandomData.RandomString();
                string type = RandomData.RandomString();
                string value = RandomData.RandomString();

                AuthorityClaim existingClaim = await TestOperations.CreateClaim(context, domainId, friendlyName, issuer, type, value);

                // Act && Assert
                await AssertExtensions.ThrowAsync<RequirementFailedException>(async () =>
                {
                    CreateClaim create = new CreateClaim(context, domainId, friendlyName, issuer, type, value);
                    AuthorityClaim claim = await create.Do();
                    await create.CommitAsync();
                }, 
                ex => ex.ErrorCode == ErrorCodes.ClaimNameNotAvailable);
            }
        }
    }
}
