using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Authority.DomainModel;
using Authority.EntityFramework;

namespace Authority.Groups
{
    public sealed class AddPolicyToGroup : OperationWithNoReturnAsync
    {
        private readonly Guid _groupId;
        private readonly Guid _policyId;

        public AddPolicyToGroup(IAuthorityContext authorityContext, Guid groupId, Guid policyId) 
            : base(authorityContext)
        {
            _groupId = groupId;
            _policyId = policyId;
        }

        public override async Task Do()
        {
            Group group = await Context.Groups
                .Include(g => g.Policies)
                .FirstOrDefaultAsync(g => g.Id == _groupId);

            Require(() => group != null, ErrorCodes.GroupNotFound);

            Policy policy = await Context.Policies.FirstOrDefaultAsync(p => p.Id == _policyId);

            Require(() => policy != null, ErrorCodes.PolicyNotFound);
            Require(() => policy.DomainId == group.DomainId, ErrorCodes.DomainMismatch);

            if (group.Policies.Any(p => p.Id == _policyId))
            {
                return;
            }

            group.Policies.Add(policy);
        }
    }
}
