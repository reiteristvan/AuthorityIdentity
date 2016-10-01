using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AuthorityIdentity.DomainModel;
using AuthorityIdentity.EntityFramework;
using AuthorityIdentity.Claims;

namespace AuthorityIdentity.Services
{
    public interface IClaimService
    {
        Task<AuthorityClaim> Create(string friendlyName, string issuer, string type, string value, Guid domainId = new Guid());
        Task Delete(Guid claimId);
        Task Update(Guid claimId, string friendlyName, string issuer, string type, string value);
        Task<List<AuthorityClaim>> All(Guid domainId = new Guid());
    }

    public sealed class ClaimService : IClaimService
    {
        public async Task<AuthorityClaim> Create(string friendlyName, string issuer, string type, string value, Guid domainId = new Guid())
        {
            if (domainId == Guid.Empty)
            {
                domainId = Common.GetDomainId();
            }

            IAuthorityContext context = AuthorityContextProvider.Create();

            CreateClaim create = new CreateClaim(context, domainId, friendlyName, issuer, type, value);
            AuthorityClaim claim = await create.Do();
            await create.CommitAsync();

            return claim;
        }

        public async Task Delete(Guid claimId)
        {
            IAuthorityContext context = AuthorityContextProvider.Create();
            DeleteClaim delete = new DeleteClaim(context, claimId);
            await delete.Do();
            await delete.CommitAsync();
        }

        public async Task Update(Guid claimId, string friendlyName, string issuer, string type, string value)
        {
            IAuthorityContext context = AuthorityContextProvider.Create();

            UpdateClaim update = new UpdateClaim(context, claimId, friendlyName, issuer, type, value);
            await update.Do();
            await update.CommitAsync();
        }

        public async Task<List<AuthorityClaim>> All(Guid domainId = new Guid())
        {
            if (domainId == Guid.Empty)
            {
                domainId = Common.GetDomainId();
            }

            IAuthorityContext context = AuthorityContextProvider.Create();
            Domain domain = await context.Domains
                .Include(d => d.Claims)
                .FirstOrDefaultAsync(d => d.Id == domainId);

            return domain.Claims.ToList();
        }
    }
}
