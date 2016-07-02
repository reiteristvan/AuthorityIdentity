using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Authority.DomainModel;
using Authority.IntegrationTests.Fixtures;
using Authority.Operations.Products;
using Xunit;

namespace Authority.IntegrationTests.Domains
{
    public sealed class DeleteDomainTests : IClassFixture<SimpleFixture>
    {
        private readonly SimpleFixture _fixture;

        public DeleteDomainTests(SimpleFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task DeleteDomainShouldSucceed()
        {
            CreateDomain createOperation = new CreateDomain(_fixture.Context, RandomData.RandomString());
            Guid domainId = await createOperation.Do();
            await createOperation.CommitAsync();

            DeleteDomain deleteOperation = new DeleteDomain(_fixture.Context, domainId);
            await deleteOperation.Execute();

            Domain domain = await _fixture.Context.Domains.FirstOrDefaultAsync(d => d.Id == domainId);

            Assert.Null(domain);
        }
    }
}
