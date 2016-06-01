using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Authority.DomainModel;
using Authority.EntityFramework;

namespace Authority.Operations.Products
{
    public class ToggleProductPublish : OperationWithReturnValueAsync<bool>
    {
        private readonly Guid _ownerId;
        private readonly Guid _productId;

        public ToggleProductPublish(IAuthorityContext context, Guid ownerId, Guid productId)
            : base(context)
        {
            _ownerId = ownerId;
            _productId = productId;
        }

        public override async Task<bool> Do()
        {
            Product product = await Context.Products
                .FirstOrDefaultAsync(p => p.Id == _productId);

            Check(() => product.OwnerId == _ownerId, ProductErrorCodes.UnAuthorizedAccess);

            product.IsPublic = !product.IsPublic;

            return product.IsPublic;
        }
    }
}
