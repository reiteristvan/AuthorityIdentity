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
    public sealed class AddClaimsToPolicy : OperationWithNoReturnAsync
    {
        private readonly Guid _policyId;
        private readonly IEnumerable<Guid> _claims;

        public AddClaimsToPolicy(IAuthorityContext authorityContext, Guid policyId, IEnumerable<Guid> claims) 
            : base(authorityContext)
        {
            _policyId = policyId;
            _claims = claims;
        }

        public override async Task Do()
        {
            Policy policy = await Context.Policies.FirstOrDefaultAsync(p => p.Id == _policyId);

            Require(() => policy != null, PolicyErrorCodes.PolicyNotFound);
        }
    }
}
