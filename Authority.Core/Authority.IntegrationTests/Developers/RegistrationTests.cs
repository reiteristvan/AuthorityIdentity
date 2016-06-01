using System.Data.Entity;
using System.Threading.Tasks;
using Authority.DomainModel;
using Authority.IntegrationTests.Common;
using Authority.IntegrationTests.Fixtures;
using Authority.Operations;
using Authority.Operations.Developers;
using Xunit;

namespace Authority.IntegrationTests.Developers
{
    public sealed class RegistrationTests : IClassFixture<SimpleFixture>
    {
        private readonly SimpleFixture _fixture;

        public RegistrationTests(SimpleFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task RegistrationShouldSuccess()
        {
            string email = RandomData.Email();
            string username = RandomData.RandomString();
            string password = RandomData.RandomString(12, true);

            DeveloperRegistration operation = new DeveloperRegistration(_fixture.Context, email, username, password);
            Developer developer = await operation.Do();
            
            await operation.CommitAsync();

            Developer developerInDb = await _fixture.Context.Developers
                .FirstOrDefaultAsync(d => d.Id == developer.Id);

            Assert.NotNull(developerInDb);
            Assert.Equal(developerInDb.DisplayName, username);
        }

        [Fact]
        public async Task RegistrationDuplicateUserShouldFail()
        {
            string email = RandomData.Email();
            string username = RandomData.RandomString();
            string password = RandomData.RandomString(12, true);

            DeveloperRegistration operation = new DeveloperRegistration(_fixture.Context, email, username, password);
            Developer developer = await operation.Do();

            await operation.CommitAsync();

            await AssertExtensions.ThrowAsync<RequirementFailedException>(async () =>
            {
                DeveloperRegistration failOperation = new DeveloperRegistration(_fixture.Context, email, username, password);
                Developer failDeveloper = await failOperation.Do();
            });          
        }

        [Fact]
        public async Task RegistrationDuplicateUsernameShouldFail()
        {
            string email = RandomData.Email();
            string username = RandomData.RandomString();
            string password = RandomData.RandomString(12, true);

            DeveloperRegistration operation = new DeveloperRegistration(_fixture.Context, email, username, password);
            Developer developer = await operation.Do();

            Assert.True(developer.Email == email);

            await operation.CommitAsync();

            await AssertExtensions.ThrowAsync<RequirementFailedException>(async () =>
            {
                string newEmail = RandomData.Email();
                DeveloperRegistration failOperation = new DeveloperRegistration(_fixture.Context, email, username, password);
                Developer failDeveloper = await failOperation.Do();
            });     
        }
    }
}
