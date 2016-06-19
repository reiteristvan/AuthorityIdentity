using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Authority.DomainModel;
using Authority.EntityFramework;
using Serilog.Events;

namespace Authority.Operations.Account
{
    public sealed class UserActivation : OperationWithNoReturnAsync
    {
        private readonly Guid _productId;
        private readonly Guid _activationCode;
        private string _email;

        public UserActivation(IAuthorityContext authorityContext, Guid productId, Guid activationCode)
            : base(authorityContext)
        {
            _productId = productId;
            _activationCode = activationCode;
        }

        public override async Task Do()
        {
            Product product = await Context.Products.FirstOrDefaultAsync(p => p.Id == _productId);

            if (_activationCode == Guid.Empty || product == null || !product.IsActive || !product.IsPublic)
            {
                throw new RequirementFailedException(AccountErrorCodes.FailedActivation);
            }

            User user = await Context.Users
                .FirstOrDefaultAsync(u => u.ProductId == product.Id && u.PendingRegistrationId == _activationCode);

            if (user == null || !user.IsPending)
            {
                throw new RequirementFailedException(AccountErrorCodes.FailedActivation);
            }

            _email = user.Email;
            user.PendingRegistrationId = Guid.Empty;
            user.IsPending = false;
        }

        public override void Commit()
        {
            base.Commit();
            Authority.Logger.Write(LogEventLevel.Information, "User activated {0}", _email);
        }

        public override async Task CommitAsync()
        {
            await base.CommitAsync();
            Authority.Logger.Write(LogEventLevel.Information, "User activated {0}", _email);
        }
    }
}
