using System;
using System.Data.Entity;
using System.Threading.Tasks;
using AuthorityIdentity.DomainModel;
using AuthorityIdentity.IntegrationTests.Common;
using AuthorityIdentity.Domains;
using Xunit;

namespace AuthorityIdentity.IntegrationTests.Domains
{
    public sealed class CreateDomainTests
    {
        [Fact]
        public async Task CreateDomainShouldSucceed()
        {
            using (AuthorityTestContext testContext = new AuthorityTestContext())
            {
                // Arrange
                string name = RandomData.RandomString();

                // Act
                CreateDomain create = new CreateDomain(testContext.Context, name);
                Guid domainId = await create.Do();
                await create.CommitAsync();

                // Assert
                Domain domain = await testContext.Context.Domains.FirstOrDefaultAsync(d => d.Id == domainId);
                Assert.NotNull(domain);
                Assert.Equal(name, domain.Name);
            }
        }

        [Fact]
        public async Task CreateDomainNameExistsShouldFail()
        {
            using (AuthorityTestContext testContext = new AuthorityTestContext())
            {
                // Arrange
                string name = RandomData.RandomString();
                CreateDomain create = new CreateDomain(testContext.Context, name);
                Guid domainId = await create.Do();
                await create.CommitAsync();

                // Act
                CreateDomain createAgain = new CreateDomain(testContext.Context, name);

                // Assert
                await AssertExtensions.ThrowAsync<RequirementFailedException>(async () =>
                {
                    Guid newId = await createAgain.Do();
                    await createAgain.CommitAsync();
                },
                    ex => ex.ErrorCode == ErrorCodes.DomainNameNotAvailable);
            }
        }
    }
}
