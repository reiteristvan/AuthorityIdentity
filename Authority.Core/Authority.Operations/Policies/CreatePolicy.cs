using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Authority.DomainModel;
using Authority.EntityFramework;

namespace Authority.Operations.Policies
{
    public class CreatePolicy : OperationWithReturnValueAsync<Policy>
    {
        private readonly Guid _domainId;
        private readonly string _name;
        private readonly bool _defaultPolicy;

        public CreatePolicy(IAuthorityContext AuthorityContext, Guid domainId, string name, bool defaultPolicy = false)
            : base(AuthorityContext)
        {
            _domainId = domainId;
            _name = name;
            _defaultPolicy = defaultPolicy;
        }

        public override async Task<Policy> Do()
        {
            Domain domain = await Context.Domains
                .FirstOrDefaultAsync(d => d.Id == _domainId);

            if (domain == null)
            {
                throw new RequirementFailedException(PolicyErrorCodes.DomainNotFound);
            }

            Policy policy = new Policy
            {
                Name = _name,
                DomainId= _domainId,
                Default = _defaultPolicy
            };

            Context.Policies.Add(policy);

            return policy;
        }
    }
}
