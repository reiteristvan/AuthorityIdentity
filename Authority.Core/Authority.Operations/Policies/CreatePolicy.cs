using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Authority.DomainModel;
using Authority.EntityFramework;

namespace Authority.Operations.Policies
{
    public class CreatePolicy : OperationWithReturnValueAsync<Policy>
    {
        private readonly Guid _productId;
        private readonly string _name;
        private readonly bool _defaultPolicy;

        public CreatePolicy(IAuthorityContext AuthorityContext, Guid productId, string name, bool defaultPolicy = false)
            : base(AuthorityContext)
        {
            _productId = productId;
            _name = name;
            _defaultPolicy = defaultPolicy;
        }

        public override async Task<Policy> Do()
        {
            Product product = await Context.Products
                .FirstOrDefaultAsync(p => p.Id == _productId);

            Policy policy = new Policy
            {
                Name = _name,
                ProductId = _productId,
                Default = _defaultPolicy
            };

            Context.Policies.Add(policy);

            return policy;
        }
    }
}
