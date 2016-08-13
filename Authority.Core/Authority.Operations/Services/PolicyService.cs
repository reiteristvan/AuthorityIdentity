using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Authority.DomainModel;
using Authority.EntityFramework;
using Authority.Operations.Policies;

namespace Authority.Operations.Services
{
    public interface IPolicyService
    {
        List<Policy> All(Guid domainId = new Guid());
        Task<Policy> FindById(Guid policyId, bool includeUsers = false);
        Task<Policy> Create(string name, Guid domainId = new Guid());
        Task<Policy> Create(string name, bool defaultPolicy, bool replaceDefault, Guid domainId = new Guid());
        Task Delete(Guid policyId);
        Task AddUser(Guid policyId, Guid userId);
        Task RemoveUser(Guid policyId, Guid userId);
        Task AddClaims(Guid policyId, IEnumerable<Guid> claimIdList);
        Task RemoveClaims(Guid policyId, IEnumerable<Guid> claimIdList);
    }

    public sealed class PolicyService : IPolicyService
    {
        public List<Policy> All(Guid domainId = new Guid())
        {
            if (domainId == Guid.Empty)
            {
                domainId = Common.GetDomainId();
            }

            IAuthorityContext context = AuthorityContextProvider.Create();
            return context.Policies.Where(p => p.DomainId == domainId).ToList();
        }

        public async Task<Policy> FindById(Guid policyId, bool includeUsers = false)
        {
            IAuthorityContext context = AuthorityContextProvider.Create();

            IQueryable<Policy> query = context.Policies
                .Include(p => p.Claims);

            if (includeUsers)
            {
                query = query.Include(p => p.Users);
            }

            Policy policy = await query.FirstOrDefaultAsync(p => p.Id == policyId);

            return policy;
        }

        public async Task<Policy> Create(string name, Guid domainId = new Guid())
        {
            return await Create(name, false, false, domainId);
        }

        public async Task<Policy> Create(string name, bool defaultPolicy, bool replaceDefault, Guid domainId = new Guid())
        {
            if (domainId == Guid.Empty)
            {
                domainId = Common.GetDomainId();
            }

            IAuthorityContext context = AuthorityContextProvider.Create();

            CreatePolicy create = new CreatePolicy(context, domainId, name, defaultPolicy, replaceDefault);
            Policy policy = await create.Do();
            await create.CommitAsync();

            return policy;
        }

        public async Task Delete(Guid policyId)
        {
            IAuthorityContext context = AuthorityContextProvider.Create();
            DeletePolicy delete = new DeletePolicy(context, policyId);
            await delete.Do();
            await delete.CommitAsync();
        }

        public async Task AddUser(Guid policyId, Guid userId)
        {
            IAuthorityContext context = AuthorityContextProvider.Create();
            AddUserToPolicy addUser = new AddUserToPolicy(context, userId, policyId);
            await addUser.Do();
            await addUser.CommitAsync();
        }

        public async Task RemoveUser(Guid policyId, Guid userId)
        {
            IAuthorityContext context = AuthorityContextProvider.Create();
            RemoveUserFromPolicy removeUser = new RemoveUserFromPolicy(context, userId, policyId);
            await removeUser.Do();
            await removeUser.CommitAsync();
        }

        public async Task AddClaims(Guid policyId, IEnumerable<Guid> claimIdList)
        {
            IAuthorityContext context = AuthorityContextProvider.Create();
            AddClaimsToPolicy addClaims = new AddClaimsToPolicy(context, policyId, claimIdList);
            await addClaims.Do();
            await addClaims.CommitAsync();
        }

        public async Task RemoveClaims(Guid policyId, IEnumerable<Guid> claimIdList)
        {
            IAuthorityContext context = AuthorityContextProvider.Create();
            RemoveClaimsFromPolicy removeClaims = new RemoveClaimsFromPolicy(context, policyId, claimIdList);
            await removeClaims.Do();
            await removeClaims.CommitAsync();
        }
    }
}
