using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Authority.DomainModel;
using Authority.EntityFramework;

namespace Authority.Operations.Policies
{
    public sealed class RemoveUserFromPolicy : OperationWithNoReturnAsync
    {
        private readonly Guid _userId;
        private readonly Guid _policyId;

        public RemoveUserFromPolicy(IAuthorityContext authorityContext, Guid userId, Guid policyId) 
            : base(authorityContext)
        {
            _userId = userId;
            _policyId = policyId;
        }

        public override async Task Do()
        {
            User user = await Context.Users
                .Include(u => u.Policies)
                .FirstOrDefaultAsync(u => u.Id == _userId);

            Require(() => user != null, PolicyErrorCodes.UnAuthorizedAccess);

            Policy policy = await Context.Policies.FirstOrDefaultAsync(p => p.Id == _policyId);

            Require(() => policy != null, PolicyErrorCodes.PolicyNotFound);
            Require(() => policy.DomainId == user.DomainId, PolicyErrorCodes.UnAuthorizedAccess);

            user.Policies.Remove(policy);
        }
    }
}
