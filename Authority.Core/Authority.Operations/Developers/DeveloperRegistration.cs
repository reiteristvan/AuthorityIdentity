using System;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using Authority.DomainModel;
using Authority.EntityFramework;
using Authority.Operations.Utilities;

namespace Authority.Operations.Developers
{
    public sealed class DeveloperRegistration : OperationWithReturnValueAsync<Developer>
    {
        private readonly string _email;
        private readonly string _displayname;
        private readonly string _password;
        private readonly PasswordService _passwordService;

        public DeveloperRegistration(IAuthorityContext AuthorityContext, string email, string displayname, string password)
            : base(AuthorityContext)
        {
            _email = email;
            _displayname = displayname;
            _password = password;
            _passwordService = new PasswordService();
        }

        private async Task<bool> IsUserNotExist(string email)
        {
            Developer user = await Context.Developers.FirstOrDefaultAsync(u => u.Email == email);
            return user == null;
        }

        private async Task<bool> IsUsernameAvailable(string displayName)
        {
            Developer user = await Context.Developers.FirstOrDefaultAsync(p => p.DisplayName == displayName);
            return user == null;
        }

        public override async Task<Developer> Do()
        {
            await Check(() => IsUserNotExist(_email), DevelopersErrorCodes.EmailAlreadyExists);
            await Check(() => IsUsernameAvailable(_displayname), DevelopersErrorCodes.DisplayNameNotAvailable);

            byte[] passwordBytes = Encoding.UTF8.GetBytes(_password);
            byte[] saltBytes = _passwordService.CreateSalt();
            byte[] hashBytes = _passwordService.CreateHash(passwordBytes, saltBytes);

            Developer developer = new Developer
            {
                Email = _email,
                DisplayName = _displayname,
                Salt = Convert.ToBase64String(saltBytes),
                PasswordHash = Convert.ToBase64String(hashBytes),
                IsActive = true,
                IsPending = true,
                PendingRegistrationId = Guid.NewGuid(),
                Created = DateTime.UtcNow
            };

            Context.Developers.Add(developer);

            return developer;
        }
    }
}
