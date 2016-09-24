using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Authority.DomainModel;
using Authority.EntityFramework;

namespace Authority.Claims
{
    public sealed class CreateClaim : OperationWithReturnValueAsync<AuthorityClaim>
    {
        private readonly Guid _domainId;
        private readonly string _friendlyName;
        private readonly string _issuer;
        private readonly string _type;
        private readonly string _value;

        public CreateClaim(IAuthorityContext context, Guid domainId, string friendlyName, string issuer, string type, string value)
            : base(context)
        {
            _domainId = domainId;
            _friendlyName = friendlyName;
            _issuer = issuer;
            _type = type;
            _value = value;
        }

        public override  async Task<AuthorityClaim> Do()
        {
            Domain domain = await Context.Domains
                .Include(d => d.Claims)
                .FirstOrDefaultAsync(d => d.Id == _domainId);

            Require(() => domain.Claims.All(c => c.FriendlyName != _friendlyName), ErrorCodes.ClaimNameNotAvailable);

            AuthorityClaim claim = new AuthorityClaim
            {
                FriendlyName = _friendlyName,
                Issuer = _issuer,
                Type = _type,
                Value = _value
            };

            domain.Claims.Add(claim);

            return claim;
        }
    }
}
