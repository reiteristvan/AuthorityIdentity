using AuthorityIdentity;
using AuthorityIdentity.DomainModel;
using AuthorityIdentity.IntegrationTests;

namespace Authority.IntegrationTests
{
    public sealed class TestTwoFactorService : ITwoFactorService
    {
        public static string LastToken = "";

        public string GenerateToken()
        {
            return RandomData.RandomString();
        }

        public void SendToken(TwoFactorType twoFactoryType, string target, string token)
        {
            LastToken = token;
        }
    }
}
