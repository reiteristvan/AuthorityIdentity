using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Authority.DomainModel;
using Authority.EntityFramework;

namespace Authority.Operations.Policies
{
    public sealed class RemoveClaimsFromPolicy : OperationWithNoReturnAsync
    {
        private readonly Guid _policyId;
        private readonly IEnumerable<Guid> _claims;

        public RemoveClaimsFromPolicy(IAuthorityContext authorityContext, Guid policyId, IEnumerable<Guid> claims) 
            : base(authorityContext)
        {
            _policyId = policyId;
            _claims = claims;
        }

        public override async Task Do()
        {
            Policy policy = await Context.Policies
                .Include(p => p.Claims)
                .FirstOrDefaultAsync(p => p.Id == _policyId);

            Require(() => policy != null, PolicyErrorCodes.PolicyNotFound);

            Domain domain = await Context.Domains
                .Include(d => d.Claims)
                .FirstOrDefaultAsync(d => d.Id == policy.DomainId);

            Require(() => _claims.All(id => domain.Claims.Any(c => c.Id == id)), PolicyErrorCodes.ClaimNotExists);

            IEnumerable<Guid> claimsToRemove = _claims.Where(id => policy.Claims.Any(cl => cl.Id == id));
            List<AuthorityClaim> claims = Context.Claims.Where(c => claimsToRemove.Contains(c.Id)).ToList();

            foreach (AuthorityClaim claim in claims)
            {
                policy.Claims.Remove(claim);
            }
        }
    }
}
