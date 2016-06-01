using System.Data.Entity;
using System.Threading.Tasks;
using Authority.DomainModel;
using Authority.IntegrationTests.Common;
using Authority.IntegrationTests.Fixtures;
using Authority.Operations.Developers;
using Xunit;

namespace Authority.IntegrationTests.Developers
{
    public sealed class LoginTests : IClassFixture<SimpleFixture>
    {
        private readonly SimpleFixture _fixture;

        public LoginTests(SimpleFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task LoginShouldSucceed()
        {
            string password = "12Budapest99";
            Developer developer = await TestOperations.RegisterAndActivateDeveloper(_fixture.Context, password);

            DeveloperLogin login = new DeveloperLogin(_fixture.Context, developer.Email, password);
            bool result = await login.Do();

            Assert.True(result);
        }

        [Fact]
        public async Task LoginNotActivatedShouldFail()
        {
            string password = "12Budapest99";
            Developer developer = await TestOperations.RegisterDeveloper(_fixture.Context, password);

            DeveloperLogin login = new DeveloperLogin(_fixture.Context, developer.Email, password);
            bool result = await login.Do();

            Assert.False(result);
        }

        [Fact]
        public async Task LoginNotActiveShouldFail()
        {
            string password = "12Budapest99";
            Developer developer = await TestOperations.RegisterAndActivateDeveloper(_fixture.Context, password);

            developer.IsActive = false;
            _fixture.Context.Entry(developer).State = EntityState.Modified;
            await _fixture.Context.SaveChangesAsync();

            DeveloperLogin login = new DeveloperLogin(_fixture.Context, developer.Email, password);
            bool result = await login.Do();

            Assert.False(result);
        }
    }
}
