using Authority.DataAccess;
using Xunit;

namespace AuthorityIdentity.IntegrationTests.DapperMigration
{
    public sealed class DatabaseInitializeTests
    {
        [Fact(Skip = "Future feature")]
        public void InitializeShouldCreateDatabase()
        {
            DataAccess.Initialize();
        }
    }
}
