using System;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Authority.DomainModel;
using Authority.EntityFramework;
using Authority.Observers;
using Authority.Security;

namespace Authority.Account
{
    public sealed class RegisterUser : OperationWithReturnValueAsync<User>
    {
        private readonly Guid _domainId;
        private readonly string _email;
        private readonly string _username;
        private readonly string _password;
        private readonly bool _needToActivate;
        private readonly PasswordService _passwordService;
        private User _user;

        public RegisterUser(IAuthorityContext authorityContext, 
            Guid domainId, string email, string username, string password, bool needToActivate = true)
            : base(authorityContext)
        {
            _domainId = domainId;
            _email = email;
            _username = username;
            _password = password;
            _needToActivate = needToActivate;
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
                    Email = _email, DomainId = _domainId, Username = _username
                }));
            }

            IPasswordValidator passwordValidator;
            if (Authority.PasswordValidators.TryGetValue(_domainId, out passwordValidator))
            {
                Require(() => passwordValidator.Validate(_password), ErrorCodes.PasswordInvalid);
            }

            await Require(() => IsUserExist(), ErrorCodes.EmailAlreadyExists);
            await Require(() => IsUsernameAvailable(), ErrorCodes.UsernameNotAvailable);

            Domain domain = await Context.Domains
                .Include(d => d.Groups)
                .Include(d => d.Policies)
                .FirstOrDefaultAsync(d => d.Id == _domainId);

            byte[] passwordBytes = Encoding.UTF8.GetBytes(_password);
            byte[] saltBytes = _passwordService.CreateSalt();
            byte[] hashBytes = _passwordService.CreateHash(passwordBytes, saltBytes);

            _user = new User
            {
                DomainId = domain.Id,
                Email = _email,
                Username = _username,
                LastLogin = DateTimeOffset.MinValue,
                Salt = Convert.ToBase64String(saltBytes),
                PasswordHash = Convert.ToBase64String(hashBytes),
                IsPending = _needToActivate,
                PendingRegistrationId = _needToActivate ? Guid.NewGuid() : Guid.Empty,
                IsActive = true,
                IsExternal = false
            };

            Context.Users.Add(_user);

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

            return _user;
        }

        public override void Commit()
        {
            base.Commit();

            if (Authority.Observers.Any())
            {
                Authority.Observers.ForEach(o => o.OnRegistered(_user));
            }

            if (Authority.EmailService != null && _needToActivate)
            {
                Authority.EmailService.SendUserActivationEmail(_user.Email, _user.PendingRegistrationId);
            }
        }

        public override async Task CommitAsync()
        {
            await base.CommitAsync();

            if (Authority.Observers.Any())
            {
                Authority.Observers.ForEach(o => o.OnRegistered(_user));
            }

            if (Authority.EmailService != null && _needToActivate)
            {
                await Authority.EmailService.SendUserActivationEmailAsync(_user.Email, _user.PendingRegistrationId);
            }
        }
    }
}
