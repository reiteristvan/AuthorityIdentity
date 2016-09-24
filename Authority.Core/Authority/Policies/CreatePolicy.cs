using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Authority.DomainModel;
using Authority.EntityFramework;

namespace Authority.Policies
{
    public class CreatePolicy : OperationWithReturnValueAsync<Policy>
    {
        private readonly Guid _domainId;
        private readonly string _name;
        private readonly bool _defaultPolicy;
        private readonly bool _replaceDefault;

        public CreatePolicy(IAuthorityContext AuthorityContext, Guid domainId, string name, bool defaultPolicy = false, bool replaceDefault = false)
            : base(AuthorityContext)
        {
            _domainId = domainId;
            _name = name;
            _defaultPolicy = defaultPolicy;
            _replaceDefault = replaceDefault;
        }

        public override async Task<Policy> Do()
        {
            Domain domain = await Context.Domains
                .FirstOrDefaultAsync(d => d.Id == _domainId);

            Require(() => domain != null, ErrorCodes.DomainNotFound);

            Policy policy = new Policy
            {
                Name = _name,
                DomainId= _domainId,
                Default = _defaultPolicy
            };

            if (_defaultPolicy)
            {
                Policy defaultPolicy = await Context.Policies
                    .FirstOrDefaultAsync(p => p.DomainId == domain.Id && p.Default);

                if (defaultPolicy != null)
                {
                    if (!_replaceDefault)
                    {
                        throw new RequirementFailedException(ErrorCodes.DefaultPolicyAlreadyExists);
                    }

                    defaultPolicy.Default = false;
                }
            }

            Context.Policies.Add(policy);

            return policy;
        }
    }
}
