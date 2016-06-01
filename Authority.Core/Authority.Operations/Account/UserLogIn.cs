using System;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using Authority.DomainModel;
using Authority.EntityFramework;
using Authority.Operations;
using Authority.Operations.Utilities;

namespace IdentityServer.UnitOfWork.Account
{
    public sealed class UserLogIn : SafeOperationWithReturnValueAsync<bool>
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

        public override async Task<bool> Do()
        {
            Product product = await Context.Products
                .FirstOrDefaultAsync(p => p.Id == _productId);

            if (product == null || !product.IsActive || !product.IsPublic)
            {
                return false;
            }

            User user = await Context.Users
                .FirstOrDefaultAsync(u => u.Email == _email && u.ProductId == product.Id);

            if (user == null || user.IsPending || !user.IsActive)
            {
                return false;
            }

            byte[] passwordBytes = Encoding.UTF8.GetBytes(_password);
            byte[] saltBytes = Convert.FromBase64String(user.Salt);
            byte[] hashBytes = _passwordService.CreateHash(passwordBytes, saltBytes);
            string hash = Convert.ToBase64String(hashBytes);

            return hash == user.PasswordHash;
        }
    }
}
