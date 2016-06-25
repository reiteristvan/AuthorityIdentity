using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Authority.DomainModel;
using Authority.EntityFramework;

namespace Authority.Operations.Products
{
    public sealed class GetDomainDetails : SafeOperationWithReturnValueAsync<Domain>
    {
        private readonly Guid _productId;

        public GetDomainDetails(ISafeAuthorityContext safeAuthorityContext, Guid productId)
            : base(safeAuthorityContext)
        {
            _productId = productId;
        }

        public override async Task<Domain> Do()
        {
            Domain product = await Context.Domains
                .Include(p => p.Policies)
                .Include(p => p.Policies.Select(po => po.Claims))
                .FirstOrDefaultAsync(p => p.Id == _productId);

            return product;
        }
    }
}
