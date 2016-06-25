using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Authority.DomainModel;
using Authority.EntityFramework;
using Authority.Operations.Extensions;

namespace Authority.Operations.Products
{
    public sealed class GetClaimsOfDomain : SafeOperation
    {
        public GetClaimsOfDomain(ISafeAuthorityContext AuthorityContext)
            : base(AuthorityContext)
        {
            
        }

        public async Task<IEnumerable<AuthorityClaim>>  Retrieve(Guid productId)
        {
            Domain product = await Context.Domains
                .Include(p => p.Policies)
                .Include(p => p.Policies.Select(po => po.Claims))
                .FirstOrDefaultAsync(p => p.Id == productId);

            return product.Policies.SelectMany(p => p.Claims).DistinctBy(c => c.Id);
        }
    }
}
