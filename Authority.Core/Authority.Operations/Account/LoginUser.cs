using System;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Authority.DomainModel;
using Authority.EntityFramework;
using Authority.Operations.Extensions;
using Authority.Operations.Observers;
using Authority.Operations.Security;

namespace Authority.Operations.Account
{
    public sealed class LoginUser : OperationWithReturnValueAsync<LoginResult>
    {
        private readonly Guid _domainId;
        private readonly string _email;
        private readonly string _password;
        private readonly PasswordService _passwordService;
        private User _user;

        public LoginUser(IAuthorityContext authorityContext, Guid domainId, string email, string password)
            : base(authorityContext)
        {
            _domainId = domainId;
            _email = email;
            _password = password;
            _passwordService = new PasswordService();
        }

        public override async Task<LoginResult> Do()
        {
            if (Authority.Observers.Any())
            {
                Authority.Observers.ForEach(o => o.OnLoggingIn(new LoginInfo
                {
                    Email = _email,
                    DomainId = _domainId
                }));
            }

            LoginResult result = new LoginResult();

            Domain product = await Context.Domains
                .FirstOrDefaultAsync(p => p.Id == _domainId);

            if (product == null || !product.IsActive)
            {
                return result;
            }

            _user = await Context.Users
                .Include(u => u.Policies)
                .Include(u => u.Policies.Select(po => po.Claims))
                .FirstOrDefaultAsync(u => u.Email == _email && u.DomainId == product.Id);

            if (_user == null || _user.IsPending || !_user.IsActive)
            {
                return result;
            }

            byte[] passwordBytes = Encoding.UTF8.GetBytes(_password);
            byte[] saltBytes = Convert.FromBase64String(_user.Salt);
            byte[] hashBytes = _passwordService.CreateHash(passwordBytes, saltBytes);
            string hash = Convert.ToBase64String(hashBytes);

            if (!hash.Equals(_user.PasswordHash))
            {
                return result;
            }

            result.Email = _email;
            result.Username = _user.Username;
            result.LastLogin = _user.LastLogin;
            result.Policies = _user.Policies.ToList();
            result.Claims = _user.Policies.SelectMany(p => p.Claims).DistinctBy(c => c.Id).ToList();

            _user.LastLogin = DateTimeOffset.UtcNow;

            return result;
        }

        public override void Commit()
        {
            base.Commit();

            if (Authority.Observers.Any())
            {
                Authority.Observers.ForEach(o => o.LoggedIn(_user));
            }
        }

        public override async Task CommitAsync()
        {
            await base.CommitAsync();

            if (Authority.Observers.Any())
            {
                Authority.Observers.ForEach(o => o.LoggedIn(_user));
            }
        }
    }
}
