using System.Threading.Tasks;
using AuthorityIdentity.DomainModel;

namespace AuthorityIdentity
{
    public abstract class ExternalIdentityProvider
    {
        public abstract string Name { get; set; }

        public abstract Task<User> Register();

        public abstract bool Login(string token);
    }
}
