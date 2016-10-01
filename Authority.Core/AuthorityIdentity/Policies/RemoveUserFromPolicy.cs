using System;
using System.Data.Entity;
using System.Threading.Tasks;
using AuthorityIdentity.DomainModel;
using AuthorityIdentity.EntityFramework;

namespace AuthorityIdentity.Policies
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

            Require(() => user != null, ErrorCodes.UserNotFound);

            Policy policy = await Context.Policies.FirstOrDefaultAsync(p => p.Id == _policyId);

            Require(() => policy != null, ErrorCodes.PolicyNotFound);
            Require(() => policy.DomainId == user.DomainId, ErrorCodes.DomainMismatch);

            user.Policies.Remove(policy);
        }
    }
}
