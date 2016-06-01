using System;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using Authority.DomainModel;
using Authority.EntityFramework;
using Authority.Operations.Utilities;
using Serilog.Events;

namespace Authority.Operations.Developers
{
    public sealed class DeveloperLogin : SafeOperationWithReturnValueAsync<bool>
    {
        private readonly string _email;
        private readonly string _password;
        private readonly PasswordService _passwordService;

        public DeveloperLogin(ISafeAuthorityContext AuthorityContext, string email, string password)
            : base(AuthorityContext)
        {
            _email = email;
            _password = password;
            _passwordService = new PasswordService();
        }

        public override async Task<bool> Do()
        {
            Developer user = await Context.Developers.FirstOrDefaultAsync(u => u.Email == _email);

            if (user == null || user.IsPending || !user.IsActive)
            {
                return false;
            }

            byte[] passwordBytes = Encoding.UTF8.GetBytes(_password);
            byte[] saltBytes = Convert.FromBase64String(user.Salt);
            byte[] hashBytes = _passwordService.CreateHash(passwordBytes, saltBytes);
            string hash = Convert.ToBase64String(hashBytes);

            bool success = user.PasswordHash == hash;

            if (!success)
            {
                Authority.Logger.Write(LogEventLevel.Information, "Developer login failed - {0}", user.Email);
            }
            else
            {
                Authority.Logger.Write(LogEventLevel.Information, "Developer logged in - {0}", user.Email);
            }

            return success;
        }
    }
}
