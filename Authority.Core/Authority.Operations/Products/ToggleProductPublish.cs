using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Authority.DomainModel;
using Authority.EntityFramework;

namespace Authority.Operations.Products
{
    public class ToggleProductPublish : OperationWithReturnValueAsync<bool>
    {
        private readonly Guid _productId;

        public ToggleProductPublish(IAuthorityContext context, Guid productId)
            : base(context)
        {
            _productId = productId;
        }

        public override async Task<bool> Do()
        {
            Product product = await Context.Products
                .FirstOrDefaultAsync(p => p.Id == _productId);

            product.IsPublic = !product.IsPublic;

            return product.IsPublic;
        }
    }
}
