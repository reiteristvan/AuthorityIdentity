using System;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Authority.DomainModel;
using Authority.EntityFramework;
using Authority.Operations;
using Authority.Operations.Account;
using Authority.Operations.Extensions;
using Authority.Operations.Utilities;

namespace IdentityServer.UnitOfWork.Account
{
    public sealed class UserLogIn : SafeOperationWithReturnValueAsync<LoginResult>
    {
        private readonly Guid _productId;
        private readonly string _email;
        private readonly string _password;
        private readonly PasswordService _passwordService;

        public UserLogIn(ISafeAuthorityContext authorityContext, Guid productId, string email, string password)
            : base(authorityContext)
        {
            _productId = productId;
            _email = email;
            _password = password;
            _passwordService = new PasswordService();
        }

        public override async Task<LoginResult> Do()
        {
            LoginResult result = new LoginResult();

            Product product = await Context.Products
                .FirstOrDefaultAsync(p => p.Id == _productId);

            if (product == null || !product.IsActive || !product.IsPublic)
            {
                return result;
            }

            User user = await Context.Users
                .Include(u => u.Policies)
                .Include(u => u.Policies.Select(po => po.Claims))
                .FirstOrDefaultAsync(u => u.Email == _email && u.ProductId == product.Id);

            if (user == null || user.IsPending || !user.IsActive)
            {
                return result;
            }

            byte[] passwordBytes = Encoding.UTF8.GetBytes(_password);
            byte[] saltBytes = Convert.FromBase64String(user.Salt);
            byte[] hashBytes = _passwordService.CreateHash(passwordBytes, saltBytes);
            string hash = Convert.ToBase64String(hashBytes);

            if (!hash.Equals(user.PasswordHash))
            {
                return result;
            }

            result.Email = _email;
            result.Username = user.Username;
            result.Policies = user.Policies.ToList();
            result.Claims = user.Policies.SelectMany(p => p.Claims).DistinctBy(c => c.Id).ToList();

            return result;
        }
    }
}
