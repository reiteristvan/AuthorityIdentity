using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Authority.DomainModel;
using Authority.IntegrationTests.Common;
using Authority.IntegrationTests.Fixtures;
using Authority.Operations;
using Authority.Operations.Products;
using Xunit;

namespace Authority.IntegrationTests.Domains
{
    public sealed class CreateDomainTests : IClassFixture<SimpleFixture>
    {
        private readonly SimpleFixture _fixture;

        public CreateDomainTests(SimpleFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task CreateDomainShouldSucceed()
        {
            // Arrange
            string name = RandomData.RandomString();

            // Act
            CreateDomain create = new CreateDomain(_fixture.Context, name);
            Guid domainId = await create.Do();
            await create.CommitAsync();

            // Assert
            Domain domain = await _fixture.Context.Domains.FirstOrDefaultAsync(d => d.Id == domainId);
            Assert.NotNull(domain);
            Assert.Equal(name, domain.Name);
        }

        [Fact]
        public async Task CreateDomainNameExistsShouldFail()
        {
            // Arrange
            string name = RandomData.RandomString();
            CreateDomain create = new CreateDomain(_fixture.Context, name);
            Guid domainId = await create.Do();
            await create.CommitAsync();

            // Act
            CreateDomain createAgain = new CreateDomain(_fixture.Context, name);

            // Assert
            await AssertExtensions.ThrowAsync<RequirementFailedException>(async () => {
                Guid newId = await createAgain.Do();
                await createAgain.CommitAsync();
            },
            ex => ex.ErrorCode == DomainErrorCodes.NameNotAvailable);
        }
    }
}
