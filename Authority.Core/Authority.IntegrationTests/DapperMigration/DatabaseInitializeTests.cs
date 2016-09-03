using Xunit;

namespace Authority.IntegrationTests.DapperMigration
{
    public sealed class DatabaseInitializeTests
    {
        [Fact]
        public void InitializeShouldCreateDatabase()
        {
            DataAccess.DataAccess.Initialize();
        }
    }
}
