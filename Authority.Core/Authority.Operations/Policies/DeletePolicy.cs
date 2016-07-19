using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Authority.DomainModel;
using Authority.EntityFramework;

namespace Authority.Operations.Policies
{
    public sealed class DeletePolicy : OperationWithNoReturnAsync
    {
        private readonly Guid _policyId;

        public DeletePolicy(IAuthorityContext authorityContext, Guid policyId) 
            : base(authorityContext)
        {
            _policyId = policyId;
        }

        public override async Task Do()
        {
            Policy policy = await Context.Policies.FirstOrDefaultAsync(p => p.Id == _policyId);

            Require(() => policy != null, PolicyErrorCodes.PolicyNotFound);

            Context.Policies.Remove(policy);
        }
    }
}
