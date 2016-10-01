using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AuthorityIdentity.DomainModel;
using AuthorityIdentity.Claims;
using AuthorityIdentity.Policies;
using Xunit;

namespace AuthorityIdentity.IntegrationTests.Policies
{
    public sealed class RemoveClaimsFromPolicyTests
    {
        [Fact]
        public async Task RemoveClaimsFromPolicyShouldSucceed()
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

                AddClaimsToPolicy addClaims = new AddClaimsToPolicy(testContext.Context, policy.Id, claims.Select(c => c.Id));
                await addClaims.Do();
                await addClaims.CommitAsync();

                // Act
                RemoveClaimsFromPolicy removeClaims = new RemoveClaimsFromPolicy(testContext.Context, policy.Id, claims.Select(c => c.Id));
                await removeClaims.Do();
                await removeClaims.CommitAsync();

                // Assert
                Guid policyId = policy.Id;
                policy = await testContext.Context.Policies.Include(p => p.Claims).FirstOrDefaultAsync(p => p.Id == policyId);

                Assert.NotNull(policy);
                Assert.True(policy.Claims.Count == 0);
            }
        }
    }
}
