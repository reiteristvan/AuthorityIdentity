using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Authority.DomainModel;
using Authority.EntityFramework;

namespace Authority.Operations.Products
{
    public sealed class CreateClaimForProduct : Operation
    {
        public CreateClaimForProduct(IAuthorityContext AuthorityContext)
            : base(AuthorityContext)
        {
            
        }

        public async Task Execute(
            Guid productId, string friendlyName, string issuer, string type, string value)
        {
            Domain product = await Context.Domains
                .Include(p => p.Policies)
                .Include(p => p.Policies.Select(po => po.Claims))
                .FirstOrDefaultAsync(p => p.Id == productId);

            AuthorityClaim claim = new AuthorityClaim
            {
                FriendlyName = friendlyName,
                Issuer = issuer,
                Type = type,
                Value = value
            };

            product.Claims.Add(claim);
            Context.Claims.Add(claim);
        }
    }
}
