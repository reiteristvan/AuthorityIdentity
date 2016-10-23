using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AuthorityIdentity.DomainModel;
using AuthorityIdentity.EntityFramework;

namespace AuthorityIdentity.Account
{
    public sealed class RegisterExternalUser : OperationWithReturnValueAsync<User>
    {
        private readonly Guid _domainId;
        private readonly string _externalIdentityProviderName;
        private readonly ExternalIdentityProvider _externalIdentityProvider;
        private User _user;

        public RegisterExternalUser(IAuthorityContext authorityContext, Guid domainId, string externalIdentityProviderName)
            : base(authorityContext)
        {
            _domainId = domainId;
            _externalIdentityProviderName = externalIdentityProviderName;
            _externalIdentityProvider = Authority.ExternalIdentityProviders
                .First(eip => eip.Name.Equals(externalIdentityProviderName, StringComparison.InvariantCultureIgnoreCase));
        }

        private async Task<bool> IsUserExist()
        {
            User user = await Context.Users
                .FirstOrDefaultAsync(u => u.Email == _user.Email && u.DomainId == _user.DomainId);
            return user == null;
        }

        private async Task<bool> IsUsernameAvailable()
        {
            User user = await Context.Users
                .FirstOrDefaultAsync(p => p.Username == _user.Username && p.DomainId == _user.DomainId);
            return user == null;
        }

        public override async Task<User> Do()
        {
            _user = await _externalIdentityProvider.Register();
            _user.ExternalProviderName = _externalIdentityProviderName;
            _user.IsExternal = true;
            _user.DomainId = _domainId;

            await Require(() => IsUserExist(), ErrorCodes.EmailAlreadyExists);
            await Require(() => IsUsernameAvailable(), ErrorCodes.UsernameNotAvailable);

            Context.Users.Add(_user);

            return _user;
        }
    }
}
