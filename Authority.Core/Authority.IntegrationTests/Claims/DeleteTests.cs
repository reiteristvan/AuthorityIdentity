using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Authority.DomainModel;
using Authority.EntityFramework;
using Authority.IntegrationTests.Common;
using Authority.Claims;
using Xunit;

namespace Authority.IntegrationTests.Claims
{
    public sealed class DeleteTests
    {
        [Fact]
        public async Task DeleteClaimShouldSucceed()
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

                CreateClaim create = new CreateClaim(context, domainId, friendlyName, issuer, type, value);
                AuthorityClaim claim = await create.Do();
                await create.CommitAsync();

                // Act
                DeleteClaim delete = new DeleteClaim(context, claim.Id);
                await delete.Do();
                await delete.CommitAsync();

                // Assert
                claim = context.ReloadEntity<AuthorityClaim>(claim.Id);
                Assert.Null(claim);

                Domain domainWithClaims = await context.Domains
                    .Include(d => d.Claims)
                    .FirstOrDefaultAsync(d => d.Id == domainId);

                Assert.True(domainWithClaims.Claims.Count == 0);
            }
        }
    }
}
