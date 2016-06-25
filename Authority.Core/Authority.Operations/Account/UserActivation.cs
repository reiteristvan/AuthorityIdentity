using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Authority.DomainModel;
using Authority.EntityFramework;

namespace Authority.Operations.Account
{
    public sealed class UserActivation : OperationWithNoReturnAsync
    {
        private readonly Guid _productId;
        private readonly Guid _activationCode;
        private User _user;

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

            _user = await Context.Users
                .FirstOrDefaultAsync(u => u.ProductId == product.Id && u.PendingRegistrationId == _activationCode);

            if (_user == null || !_user.IsPending)
            {
                throw new RequirementFailedException(AccountErrorCodes.FailedActivation);
            }

            _user.PendingRegistrationId = Guid.Empty;
            _user.IsPending = false;
        }

        public override void Commit()
        {
            base.Commit();

            if (Authority.Observers.Any())
            {
                Authority.Observers.ForEach(o => o.OnActivated(_user));
            }
        }

        public override async Task CommitAsync()
        {
            await base.CommitAsync();

            if (Authority.Observers.Any())
            {
                Authority.Observers.ForEach(o => o.OnActivated(_user));
            }
        }
    }
}
