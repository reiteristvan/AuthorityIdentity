using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Authority.DomainModel;
using Authority.Operations.Claims;
using Authority.Operations.Policies;
using Xunit;

namespace Authority.IntegrationTests.Policies
{
    public sealed class AddClaimsToPolicyTests
    {
        [Fact]
        public async Task AddClaimsToPolicyShouldSucceed()
        {
            using (AuthorityTestContext testContext = new AuthorityTestContext())
            {
                // Arrange
                List<AuthorityClaim> claims = new List<AuthorityClaim>();
                for (int i = 0; i < 100; ++i)
                {
                    CreateClaim createClaim = new CreateClaim(testContext.Context, testContext.Domain.Id, 
                        RandomData.RandomString(), RandomData.RandomString(), RandomData.RandomString(), RandomData.RandomString());
                    AuthorityClaim claim = await createClaim.Do();
                    await createClaim.CommitAsync();

                    claims.Add(claim);
                }

                CreatePolicy createPolicy = new CreatePolicy(testContext.Context, testContext.Domain.Id, RandomData.RandomString());
                Policy policy = await createPolicy.Do();
                await createPolicy.CommitAsync();

                // Act
                AddClaimsToPolicy addClaims = new AddClaimsToPolicy(testContext.Context, policy.Id, claims.Select(c => c.Id));
                await addClaims.Do();
                await addClaims.CommitAsync();

                // Assert
                Guid policyId = policy.Id;
                policy = await testContext.Context.Policies.Include(p => p.Claims).FirstOrDefaultAsync(p => p.Id == policyId);

                Assert.NotNull(policy);
                Assert.True(policy.Claims.Count == claims.Count);
            }
        }
    }
}
