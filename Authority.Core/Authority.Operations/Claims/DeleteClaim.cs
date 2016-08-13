using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Authority.DomainModel;
using Authority.EntityFramework;

namespace Authority.Operations.Claims
{
    public sealed class DeleteClaim : OperationWithNoReturnAsync
    {
        private readonly Guid _domainId;
        private readonly Guid _claimId;

        public DeleteClaim(IAuthorityContext authorityContext, Guid domainId, Guid claimId) 
            : base(authorityContext)
        {
            _domainId = domainId;
            _claimId = claimId;
        }

        public override async Task Do()
        {
            Domain domain = await Context.Domains
                .Include(d => d.Claims)
                .FirstOrDefaultAsync(d => d.Id == _domainId);

            Require(() => domain != null, ErrorCodes.DomainNotFound);

            AuthorityClaim claim = domain.Claims.FirstOrDefault(c => c.Id == _claimId);

            Require(() => claim != null, ErrorCodes.ClaimNotFound);

            Context.Claims.Remove(claim);
        }
    }
}
