using System.Threading.Tasks;
using AuthorityIdentity;
using AuthorityIdentity.DomainModel;
using AuthorityIdentity.IntegrationTests;

namespace Authority.IntegrationTests
{
    public sealed class TestExternalProvider : ExternalIdentityProvider
    {
        public static string ProviderName = "TestExternalProvider";
        public static User LastUser;

        public override string Name => ProviderName;

        public override async Task<User> Register(object tokenParameter)
        {
            // Because of ReLogin tests
            if (LastUser != null)
            {
                return LastUser;
            }

            string email = RandomData.Email();
            LastUser = new User
            {
                Email = email,
                ExternalToken = RandomData.RandomString(),
                Username = email
            };

            return LastUser;
        }
    }
}
