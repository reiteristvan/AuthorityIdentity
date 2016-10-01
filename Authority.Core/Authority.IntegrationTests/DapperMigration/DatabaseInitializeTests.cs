using Authority.DataAccess;
using Xunit;

namespace AuthorityIdentity.IntegrationTests.DapperMigration
{
    public sealed class DatabaseInitializeTests
    {
        [Fact]
        public void InitializeShouldCreateDatabase()
        {
            DataAccess.Initialize();
        }
    }
}
