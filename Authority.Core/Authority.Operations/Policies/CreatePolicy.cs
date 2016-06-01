using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Authority.DomainModel;
using Authority.EntityFramework;

namespace Authority.Operations.Policies
{
    public class CreatePolicy : OperationWithReturnValueAsync<Policy>
    {
        private readonly Guid _userId;
        private readonly Guid _productId;
        private readonly string _name;

        public CreatePolicy(IAuthorityContext AuthorityContext, Guid userId, Guid productId, string name)
            : base(AuthorityContext)
        {
            _userId = userId;
            _productId = productId;
            _name = name;
        }

        public bool IsUserOwnsProduct(Guid userId, Product product)
        {
            if (product == null)
            {
                return false;
            }

            return product.OwnerId == userId;
        }

        public override async Task<Policy> Do()
        {
            Product product = await Context.Products
                .FirstOrDefaultAsync(p => p.Id == _productId);

            Check(() => IsUserOwnsProduct(_userId, product), PolicyErrorCodes.UnAuthorizedAccess);

            Policy policy = new Policy
            {
                Name = _name,
                ProductId = product.Id,
                Default = false
            };

            Context.Policies.Add(policy);

            return policy;
        }
    }
}
