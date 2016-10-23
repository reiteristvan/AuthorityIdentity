using System;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthorityIdentity.Observers;
using AuthorityIdentity.Security;
using AuthorityIdentity.DomainModel;
using AuthorityIdentity.EntityFramework;

namespace AuthorityIdentity.Account
{
    public sealed class RegisterUserModel
    {
        public Guid DomainId { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool NeedToActivate { get; set; }
        public string Metadata { get; set; }
    }

    public sealed class RegisterUser : OperationWithReturnValueAsync<User>
    {
        private readonly RegisterUserModel _model;
        private readonly PasswordService _passwordService;
        private User _user;

        public RegisterUser(IAuthorityContext authorityContext, RegisterUserModel model)
            : base(authorityContext)
        {
            _model = model;
            _passwordService = new PasswordService();
        }

        private async Task<bool> IsUserExist()
        {
            User user = await Context.Users
                .FirstOrDefaultAsync(u => u.Email == _model.Email && u.DomainId == _model.DomainId);
            return user == null;
        }

        private async Task<bool> IsUsernameAvailable()
        {
            User user = await Context.Users
                .FirstOrDefaultAsync(p => p.Username == _model.Username && p.DomainId == _model.DomainId);
            return user == null;
        }

        public override async Task<User> Do()
        {
            if (Authority.Observers.Any())
            {
                Authority.Observers.ForEach(o => o.OnRegistering(new RegistrationInfo
                {
                    Email = _model.Email, DomainId = _model.DomainId, Username = _model.Username
                }));
            }

            IPasswordValidator passwordValidator;
            if (Authority.PasswordValidators.TryGetValue(_model.DomainId, out passwordValidator))
            {
                Require(() => passwordValidator.Validate(_model.Password), ErrorCodes.PasswordInvalid);
            }

            await Require(() => IsUserExist(), ErrorCodes.EmailAlreadyExists);
            await Require(() => IsUsernameAvailable(), ErrorCodes.UsernameNotAvailable);

            Domain domain = await Context.Domains
                .Include(d => d.Groups)
                .Include(d => d.Policies)
                .FirstOrDefaultAsync(d => d.Id == _model.DomainId);

            byte[] passwordBytes = Encoding.UTF8.GetBytes(_model.Password);
            byte[] saltBytes = _passwordService.CreateSalt();
            byte[] hashBytes = _passwordService.CreateHash(passwordBytes, saltBytes);

            _user = new User
            {
                DomainId = domain.Id,
                Email = _model.Email,
                Username = _model.Username,
                LastLogin = DateTimeOffset.MinValue,
                Salt = Convert.ToBase64String(saltBytes),
                PasswordHash = Convert.ToBase64String(hashBytes),
                IsPending = !_model.NeedToActivate,
                PendingRegistrationId = !_model.NeedToActivate ? Guid.NewGuid() : Guid.Empty,
                IsActive = true,
                IsExternal = false,
                IsTwoFactorEnabled = false,
                TwoFactorToken = "",
                TwoFactorType = TwoFactorType.Other,
                TwoFactorTarget = ""
            };

            Metadata metaData = new Metadata(_user.Id)
            {
                Data = _model.Metadata ?? ""
            };

            Context.Users.Add(_user);
            _user.Metadata = metaData;
            Context.Metadata.Add(metaData);

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

            if (Authority.EmailService != null && _model.NeedToActivate)
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

            if (Authority.EmailService != null && _model.NeedToActivate)
            {
                await Authority.EmailService.SendUserActivationEmailAsync(_user.Email, _user.PendingRegistrationId);
            }
        }
    }
}
