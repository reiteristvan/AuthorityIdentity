using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Authority.DomainModel;
using Authority.EntityFramework;

namespace Authority.Policies
{
    public sealed class AddClaimsToPolicy : OperationWithNoReturnAsync
    {
        private readonly Guid _policyId;
        private readonly IEnumerable<Guid> _claims;

        public AddClaimsToPolicy(IAuthorityContext authorityContext, Guid policyId, IEnumerable<Guid> claims) 
            : base(authorityContext)
        {
            _policyId = policyId;
            _claims = claims.Distinct();
        }

        public override async Task Do()
        {
            Policy policy = await Context.Policies
                .Include(p => p.Claims)
                .FirstOrDefaultAsync(p => p.Id == _policyId);

            Require(() => policy != null, ErrorCodes.PolicyNotFound);

            Domain domain = await Context.Domains
                .Include(d => d.Claims)
                .FirstOrDefaultAsync(d => d.Id == policy.DomainId);

            Require(() => _claims.All(id => domain.Claims.Any(c => c.Id == id)), ErrorCodes.ClaimNotFound);

            IEnumerable<Guid> claimsToAdd = _claims.Where(id => policy.Claims.All(cl => cl.Id != id));
            List<AuthorityClaim> claims = Context.Claims.Where(c => claimsToAdd.Contains(c.Id)).ToList();

            foreach (AuthorityClaim claim in claims)
            {
                policy.Claims.Add(claim);
            }
        }
    }
}
