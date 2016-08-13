using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Authority.DomainModel;
using Authority.EntityFramework;

namespace Authority.Operations.Policies
{
    public sealed class AddUserToPolicy : OperationWithNoReturnAsync
    {
        private readonly Guid _userId;
        private readonly Guid _policyId;

        public AddUserToPolicy(IAuthorityContext authorityContext, Guid userId, Guid policyId) 
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
            Require(() => policy.DomainId == user.DomainId, ErrorCodes.DomainNotFound);

            if (user.Policies.Any(p => p.Id == _policyId))
            {
                return;
            }

            user.Policies.Add(policy);
        }
    }
}
