using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Authority.DomainModel;
using Authority.EntityFramework;

namespace Authority.Operations.Claims
{
    public sealed class UpdateClaim : OperationWithNoReturnAsync
    {
        private readonly Guid _claimId;
        private readonly string _friendlyName;
        private readonly string _issuer;
        private readonly string _type;
        private readonly string _value;

        public UpdateClaim(IAuthorityContext authorityContext, Guid claimId, string friendlyName, string issuer, string type, string value) 
            : base(authorityContext)
        {
            _claimId = claimId;
            _friendlyName = friendlyName;
            _issuer = issuer;
            _type = type;
            _value = value;
        }

        public override async Task Do()
        {
            AuthorityClaim claim = await Context.Claims.FirstOrDefaultAsync(c => c.Id == _claimId);

            Require(() => claim != null, ErrorCodes.ClaimNotFound);

            claim.FriendlyName = _friendlyName;
            claim.Issuer = _issuer;
            claim.Type = _type;
            claim.Value = _value;
        }
    }
}
