using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AuthorityIdentity.Policies;
using AuthorityIdentity.DomainModel;
using AuthorityIdentity.EntityFramework;

namespace AuthorityIdentity.Services
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
        /// <summary>
        /// Returns the list of policies of a domain. The returned entities does not include Claims and Users.
        /// </summary>
        /// <param name="domainId">The domain which contains the policy. Leave default value in single domain environment.</param>
        /// <returns>List of Policy entities</returns>
        public List<Policy> All(Guid domainId = new Guid())
        {
            if (domainId == Guid.Empty)
            {
                domainId = Common.GetDomainId();
            }

            IAuthorityContext context = AuthorityContextProvider.Create();
            return context.Policies.Where(p => p.DomainId == domainId).ToList();
        }

        /// <summary>
        /// Finds a policy by its Id. The returned entity contains the related Claim entities.
        /// </summary>
        /// <param name="policyId">Id of the policy</param>
        /// <param name="includeUsers">Users should be included</param>
        /// <returns>A Policy entity</returns>
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

        /// <summary>
        /// Creates a new Policy. The new policy won't be default.
        /// </summary>
        /// <param name="name">Name of the policy</param>
        /// <param name="domainId">Id of the domain which contains the policy. Leave default value in single domain environment.</param>
        /// <returns>The created Policy entity</returns>
        public async Task<Policy> Create(string name, Guid domainId = new Guid())
        {
            return await Create(name, false, false, domainId);
        }

        /// <summary>
        /// Creates a new Policy
        /// </summary>
        /// <param name="name">Name of the policy</param>
        /// <param name="defaultPolicy">Indicates if the policy should be default (i.e. it should be assigned to new users)</param>
        /// <param name="replaceDefault">If the defaultPolicy parameter true the function will replace the existing default policy</param>
        /// <param name="domainId">Id of the domain which contains the policy. Leave default value in single domain environment.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Deletes a Policy.
        /// </summary>
        /// <param name="policyId">Id of the Policy entity.</param>
        /// <returns></returns>
        public async Task Delete(Guid policyId)
        {
            IAuthorityContext context = AuthorityContextProvider.Create();
            DeletePolicy delete = new DeletePolicy(context, policyId);
            await delete.Do();
            await delete.CommitAsync();
        }

        /// <summary>
        /// Add a user to a Policy
        /// </summary>
        /// <param name="policyId">Id of the policy</param>
        /// <param name="userId">Id of the user</param>
        /// <returns></returns>
        public async Task AddUser(Guid policyId, Guid userId)
        {
            IAuthorityContext context = AuthorityContextProvider.Create();
            AddUserToPolicy addUser = new AddUserToPolicy(context, userId, policyId);
            await addUser.Do();
            await addUser.CommitAsync();
        }

        /// <summary>
        /// Removes a user from a policy.
        /// </summary>
        /// <param name="policyId">Id of the policy.</param>
        /// <param name="userId">Id of the user.</param>
        /// <returns></returns>
        public async Task RemoveUser(Guid policyId, Guid userId)
        {
            IAuthorityContext context = AuthorityContextProvider.Create();
            RemoveUserFromPolicy removeUser = new RemoveUserFromPolicy(context, userId, policyId);
            await removeUser.Do();
            await removeUser.CommitAsync();
        }

        /// <summary>
        /// Add claims to a policy.
        /// </summary>
        /// <param name="policyId">Id of the policy.</param>
        /// <param name="claimIdList">List of claim ids which are added to the policy.</param>
        /// <returns></returns>
        public async Task AddClaims(Guid policyId, IEnumerable<Guid> claimIdList)
        {
            IAuthorityContext context = AuthorityContextProvider.Create();
            AddClaimsToPolicy addClaims = new AddClaimsToPolicy(context, policyId, claimIdList);
            await addClaims.Do();
            await addClaims.CommitAsync();
        }

        /// <summary>
        /// Remove claims from a policy.
        /// </summary>
        /// <param name="policyId">Id of the policy.</param>
        /// <param name="claimIdList">List of claim ids which are to be removed from the policy.</param>
        /// <returns></returns>
        public async Task RemoveClaims(Guid policyId, IEnumerable<Guid> claimIdList)
        {
            IAuthorityContext context = AuthorityContextProvider.Create();
            RemoveClaimsFromPolicy removeClaims = new RemoveClaimsFromPolicy(context, policyId, claimIdList);
            await removeClaims.Do();
            await removeClaims.CommitAsync();
        }
    }
}
