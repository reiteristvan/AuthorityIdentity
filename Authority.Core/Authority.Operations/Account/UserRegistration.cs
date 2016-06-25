using System;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Authority.DomainModel;
using Authority.EntityFramework;
using Authority.Operations.Observers;
using Authority.Operations.Security;

namespace Authority.Operations.Account
{
    public sealed class UserRegistration : OperationWithReturnValueAsync<User>
    {
        private readonly Guid _domainId;
        private readonly string _email;
        private readonly string _username;
        private readonly string _password;
        private readonly PasswordService _passwordService;
        private User _user;

        public UserRegistration(IAuthorityContext authorityContext, 
            Guid domainId, string email, string username, string password)
            : base(authorityContext)
        {
            _domainId = domainId;
            _email = email;
            _username = username;
            _password = password;
            _passwordService = new PasswordService();
        }

        private async Task<bool> IsUserExist()
        {
            User user = await Context.Users
                .FirstOrDefaultAsync(u => u.Email == _email && u.DomainId == _domainId);
            return user == null;
        }

        private async Task<bool> IsUsernameAvailable()
        {
            User user = await Context.Users
                .FirstOrDefaultAsync(p => p.Username == _username && p.DomainId == _domainId);
            return user == null;
        }

        public override async Task<User> Do()
        {
            if (Authority.Observers.Any())
            {
                Authority.Observers.ForEach(o => o.OnRegistering(new RegistrationInfo
                {
                    Email = _email, ProductId = _domainId, Username = _username
                }));
            }

            await Check(() => IsUserExist(), AccountErrorCodes.EmailAlreadyExists);
            await Check(() => IsUsernameAvailable(), AccountErrorCodes.UsernameNotAvailable);

            Domain product = await Context.Domains
                .Include(p => p.Policies)
                .FirstOrDefaultAsync(p => p.Id == _domainId);

            byte[] passwordBytes = Encoding.UTF8.GetBytes(_password);
            byte[] saltBytes = _passwordService.CreateSalt();
            byte[] hashBytes = _passwordService.CreateHash(passwordBytes, saltBytes);

            _user = new User
            {
                DomainId = product.Id,
                Email = _email,
                Username = _username,
                Salt = Convert.ToBase64String(saltBytes),
                PasswordHash = Convert.ToBase64String(hashBytes),
                IsPending = true,
                PendingRegistrationId = Guid.NewGuid(),
                IsActive = true,
                IsExternal = false
            };

            Context.Users.Add(_user);

            Policy defaultPolicy = product.Policies.FirstOrDefault(p => p.Default);

            if (defaultPolicy != null)
            {
                _user.Policies.Add(defaultPolicy);
            }

            return _user;
        }

        public override void Commit()
        {
            base.Commit();

            if (Authority.Observers.Any())
            {
                Authority.Observers.ForEach(o => o.OnRegistered(_user));
            }
        }

        public override async Task CommitAsync()
        {
            await base.CommitAsync();

            if (Authority.Observers.Any())
            {
                Authority.Observers.ForEach(o => o.OnRegistered(_user));
            }
        }
    }
}
