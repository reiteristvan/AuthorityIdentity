using System.Threading.Tasks;
using Authority.DomainModel;
using Authority.IntegrationTests.Common;
using Authority.IntegrationTests.Fixtures;
using Authority.Operations.Policies;
using Xunit;

namespace Authority.IntegrationTests.Policies
{
    public sealed class CreateTest : IClassFixture<PolicyTestFixture>
    {
        private readonly PolicyTestFixture _fixture;

        public CreateTest(PolicyTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task CreatePolicyShouldSucceed()
        {
            CreatePolicy create = new CreatePolicy(
                _fixture.Context, 
                _fixture.Developer.Id, 
                _fixture.Product.Id, 
                RandomData.RandomString(), 
                true);

            Policy policy = await create.Do();
            await create.CommitAsync();

            policy = _fixture.Context.ReloadEntity<Policy>(policy.Id);

            Assert.NotNull(policy);
        }
    }
}
