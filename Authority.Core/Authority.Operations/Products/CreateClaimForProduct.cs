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
            Guid userId, Guid productId, string friendlyName, string issuer, string type, string value)
        {
            Product product = await Context.Products
                .Include(p => p.Policies)
                .Include(p => p.Policies.Select(po => po.Claims))
                .FirstOrDefaultAsync(p => p.Id == productId);

            Check(() => product.OwnerId == userId, ProductErrorCodes.UnAuthorizedAccess);

            Claim claim = new Claim
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
