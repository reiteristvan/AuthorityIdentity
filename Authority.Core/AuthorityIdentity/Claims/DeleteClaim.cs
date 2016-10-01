using System;
using System.Data.Entity;
using System.Threading.Tasks;
using AuthorityIdentity.DomainModel;
using AuthorityIdentity.EntityFramework;

namespace AuthorityIdentity.Claims
{
    public sealed class DeleteClaim : OperationWithNoReturnAsync
    {
        private readonly Guid _claimId;

        public DeleteClaim(IAuthorityContext authorityContext, Guid claimId) 
            : base(authorityContext)
        {
            _claimId = claimId;
        }

        public override async Task Do()
        {
            AuthorityClaim claim = await Context.Claims.FirstOrDefaultAsync(c => c.Id == _claimId);

            Require(() => claim != null, ErrorCodes.ClaimNotFound);

            Context.Claims.Remove(claim);
        }
    }
}
