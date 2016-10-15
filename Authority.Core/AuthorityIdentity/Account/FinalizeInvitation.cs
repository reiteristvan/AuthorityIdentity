using System;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthorityIdentity.Security;
using AuthorityIdentity.DomainModel;
using AuthorityIdentity.EntityFramework;

namespace AuthorityIdentity.Account
{
    public sealed class FinalizeInvitation : OperationWithNoReturnAsync
    {
        private readonly PasswordService _passwordService;
        private readonly Guid _invitationCode;
        private readonly string _username;
        private readonly string _password;
        private string _email;
        private User _user;

        public FinalizeInvitation(IAuthorityContext context, Guid invitationCode, string username, string password)
            : base(context)
        {
            _passwordService = new PasswordService();
            _invitationCode = invitationCode;
            _username = username;
            _password = password;
        }

        private async Task<bool> IsUsernameAvailable(Guid domainId)
        {
            User user = await Context.Users
                .FirstOrDefaultAsync(p => p.Username == _username && p.DomainId == domainId);
            return user == null;
        }

        public override async Task Do()
        {
            Invite invite = await Context.Invites.FirstOrDefaultAsync(i => i.Id == _invitationCode);

            Require(() => invite != null, ErrorCodes.InviteNotFound);
            Require(() => invite.Accepted == false, ErrorCodes.InviteAlreadyAccepted);
            Require(() => invite.Expire == null || (invite.Expire != null && invite.Expire >= DateTimeOffset.UtcNow), ErrorCodes.InviteExpired);

            Domain domain = await Context.Domains.FirstOrDefaultAsync(d => d.Id == invite.DomainId);

            Require(() => domain != null && domain.IsActive, ErrorCodes.DomainNotAvailable);

            User user = await Context.Users.FirstOrDefaultAsync(u => u.DomainId == domain.Id && u.Email == _email);

            Require(() => user == null, ErrorCodes.InviteAlreadyAccepted);
            await Require(() => IsUsernameAvailable(domain.Id), ErrorCodes.UsernameNotAvailable);

            IPasswordValidator passwordValidator;
            if (Authority.PasswordValidators.TryGetValue(invite.DomainId, out passwordValidator))
            {
                Require(() => passwordValidator.Validate(_password), ErrorCodes.PasswordInvalid);
            }

            _email = invite.Email;
            invite.Accepted = true;

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
                IsPending = false,
                PendingRegistrationId = Guid.Empty,
                IsActive = true,
                IsExternal = false,
                IsTwoFactorEnabled = false,
                TwoFactorToken = "",
                TwoFactorType = TwoFactorType.Other,
                TwoFactorTarget = ""
            };

            Context.Users.Add(_user);

            Policy defaultPolicy = domain.Policies.FirstOrDefault(p => p.Default);

            if (defaultPolicy != null)
            {
                _user.Policies.Add(defaultPolicy);
            }
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
