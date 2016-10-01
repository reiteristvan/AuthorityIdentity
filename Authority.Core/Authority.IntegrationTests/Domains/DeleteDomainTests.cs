using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AuthorityIdentity.Domains;
using AuthorityIdentity.DomainModel;
using AuthorityIdentity.IntegrationTests.Common;
using Xunit;

namespace AuthorityIdentity.IntegrationTests.Domains
{
    public sealed class DeleteDomainTests
    {
        [Fact]
        public async Task DeleteDomainShouldSucceed()
        {
            using (AuthorityTestContext testContext = new AuthorityTestContext())
            {
                CreateDomain createOperation = new CreateDomain(testContext.Context, RandomData.RandomString());
                Guid domainId = await createOperation.Do();
                await createOperation.CommitAsync();

                DeleteDomain deleteOperation = new DeleteDomain(testContext.Context, domainId);
                await deleteOperation.Execute();

                Domain domain = await testContext.Context.Domains.FirstOrDefaultAsync(d => d.Id == domainId);

                Assert.Null(domain);
            }
        }

        [Fact]
        public async Task DeleteDomainWithUsersShouldSucceed()
        {
            using (AuthorityTestContext testContext = new AuthorityTestContext())
            {
                CreateDomain createOperation = new CreateDomain(testContext.Context, RandomData.RandomString());
                Guid domainId = await createOperation.Do();
                await createOperation.CommitAsync();

                // TODO: bulk register
                for (int i = 0; i < 100; ++i)
                {
                    await TestOperations.RegisterAndActivateUser(testContext.Context, domainId, RandomData.RandomString(12, true));
                }

                DeleteDomain deleteOperation = new DeleteDomain(testContext.Context, domainId);
                await deleteOperation.Execute();

                Domain domain = await testContext.Context.Domains.FirstOrDefaultAsync(d => d.Id == domainId);

                Assert.Null(domain);

                List<User> users = testContext.Context.Users
                    .Where(u => u.DomainId == domainId)
                    .ToList();

                Assert.False(users.Any());
            }
        }

        [Fact]
        public async Task DeleteDomainWithComplexSetupShouldSucceed()
        {
            using (AuthorityTestContext testContext = new AuthorityTestContext())
            {
                // Arrange
                await TestOperations.CreateComplexSetup(testContext.Context, testContext.Domain.Id);

                // Act
                DeleteDomain deleteOperation = new DeleteDomain(testContext.Context, testContext.Domain.Id);
                await deleteOperation.Execute();

                // Assert
                Domain domain = await testContext.Context.Domains.FirstOrDefaultAsync(d => d.Id == testContext.Domain.Id);

                Assert.Null(domain);

                List<User> users = testContext.Context.Users
                    .Where(u => u.DomainId == testContext.Domain.Id)
                    .ToList();

                Assert.Empty(users);
            }
        }

        [Fact]
        public async Task DeleteDomainInMultiDomainSetupShouldSucceed()
        {
            using (AuthorityTestContext testContext = new AuthorityTestContext())
            {
                // Arrange
                const int numberOfDomains = 12;
                const int numberOfClaims = 32;
                const int numberOfPolicies = 22;
                const int numberOfUsers = 22;
                const int numberOfPolicyClaimAssociations = 12;

                await TestOperations.CreateMultiDomainSetup(testContext.Context, numberOfDomains, numberOfClaims, numberOfPolicies, numberOfUsers, numberOfPolicyClaimAssociations);

                // Act
                DeleteDomain deleteOperation = new DeleteDomain(testContext.Context, testContext.Domain.Id);
                await deleteOperation.Execute();

                // Assert
                Domain domain = await testContext.Context.Domains.FirstOrDefaultAsync(d => d.Id == testContext.Domain.Id);

                Assert.Null(domain);

                List<User> users = testContext.Context.Users
                    .Where(u => u.DomainId == testContext.Domain.Id)
                    .ToList();

                Assert.Empty(users);

                List<Domain> existingDomains = testContext.Context.Domains
                    .Include(d => d.Claims)
                    .Include(d => d.Policies)
                    .Include(d => d.Policies.Select(p => p.Claims))
                    .ToList();

                Assert.NotEmpty(existingDomains);

                foreach (Domain existingDomain in existingDomains)
                {
                    Assert.Equal(existingDomain.Policies.Count, numberOfPolicies);
                    Assert.Equal(existingDomain.Claims.Count, numberOfClaims);
                    Assert.True(existingDomain.Policies.All(p => p.Claims.Count == numberOfPolicyClaimAssociations));
                }
            }
        }
    }
}
