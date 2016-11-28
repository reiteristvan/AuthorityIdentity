using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AuthorityIdentity.DomainModel;
using AuthorityIdentity.EntityFramework;

namespace AuthorityIdentity.Account
{
    public sealed class LoginWithExternalUser : OperationWithReturnValueAsync<User>
    {
        private readonly Guid _domainId;
        private readonly string _externalIdentityProviderName;
        private readonly object _tokenParameter;
        private readonly ExternalIdentityProvider _externalIdentityProvider;
        private User _user;

        public LoginWithExternalUser(IAuthorityContext authorityContext, Guid domainId, string externalIdentityProviderName, object tokenParameter)
            : base(authorityContext)
        {
            _domainId = domainId;
            _externalIdentityProviderName = externalIdentityProviderName;
            _tokenParameter = tokenParameter;
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
            Domain domain = await Context.Domains.FirstOrDefaultAsync(d => d.Id == _domainId);

            Require(() => domain != null, ErrorCodes.DomainNotFound);

            _user = await _externalIdentityProvider.Register(_tokenParameter);

            User existingUser = await Context.Users.FirstOrDefaultAsync(u => u.Email == _user.Email);

            bool existingOverride = false;
            if (existingUser != null)
            {
                _user = existingUser;
                existingOverride = true;
            }

            _user.ExternalProviderName = _externalIdentityProviderName;
            _user.IsExternal = true;
            _user.DomainId = _domainId;
            _user.IsTwoFactorEnabled = false;
            _user.TwoFactorType = TwoFactorType.Other;
            _user.TwoFactorToken = "";
            _user.TwoFactorTarget = "";
            _user.IsActive = true;
            _user.IsPending = false;
            _user.PendingRegistrationId = Guid.Empty;
            _user.PasswordHash = "";
            _user.Salt = "";

            if (!existingOverride)
            {
                await Require(() => IsUserExist(), ErrorCodes.EmailAlreadyExists);
                await Require(() => IsUsernameAvailable(), ErrorCodes.UsernameNotAvailable);

                Policy defaultPolicy = domain.Policies.FirstOrDefault(p => p.Default);

                if (defaultPolicy != null)
                {
                    _user.Policies.Add(defaultPolicy);
                }

                Group defaultGroup = domain.Groups.FirstOrDefault(g => g.Default);

                if (defaultGroup != null)
                {
                    _user.Groups.Add(defaultGroup);
                }

                Context.Users.Add(_user);

                return _user;
            }

            Update(_user);
            return _user;
        }
    }
}
