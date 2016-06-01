using System;
using System.Threading.Tasks;
using Authority.DomainModel;
using Authority.IntegrationTests.Common;
using Authority.IntegrationTests.Fixtures;
using Authority.Operations;
using Authority.Operations.Developers;
using Xunit;

namespace Authority.IntegrationTests.Developers
{
    public sealed class ActivationTests : IClassFixture<SimpleFixture>
    {
        private readonly SimpleFixture _fixture;

        public ActivationTests(SimpleFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task ActivationShouldSuccess()
        {
            Developer developer = await TestOperations.RegisterDeveloper(_fixture.Context);

            DeveloperActivation activationOperation = new DeveloperActivation(_fixture.Context, developer.PendingRegistrationId);
            await activationOperation.Do();

            await activationOperation.CommitAsync();

            Developer existingDeveloper = _fixture.Context.ReloadEntity<Developer>(developer.Id);

            Assert.False(existingDeveloper.IsPending);
            Assert.True(existingDeveloper.PendingRegistrationId == Guid.Empty);
        }

        [Fact]
        public async Task ActivationInvalidActivationCodeShouldFail()
        {
            await AssertExtensions.ThrowAsync<RequirementFailedException>(async () =>
            {
                DeveloperActivation operation = new DeveloperActivation(_fixture.Context, Guid.Empty);
                await operation.Do();
            }); 
        }
    }
}
