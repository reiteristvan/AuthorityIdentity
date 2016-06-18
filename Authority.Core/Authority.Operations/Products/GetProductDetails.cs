using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Authority.DomainModel;
using Authority.EntityFramework;

namespace Authority.Operations.Products
{
    public sealed class GetProductDetails : SafeOperationWithReturnValueAsync<Product>
    {
        private readonly Guid _productId;

        public GetProductDetails(ISafeAuthorityContext safeAuthorityContext, Guid productId)
            : base(safeAuthorityContext)
        {
            _productId = productId;
        }

        public override async Task<Product> Do()
        {
            Product product = await Context.Products
                .Include(p => p.Policies)
                .Include(p => p.Policies.Select(po => po.Claims))
                .FirstOrDefaultAsync(p => p.Id == _productId);

            return product;
        }
    }
}
