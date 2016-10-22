using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthorityIdentity.DomainModel;
using AuthorityIdentity.EntityFramework;
using AuthorityIdentity.Groups;

namespace AuthorityIdentity.Services
{
    public interface IGroupService
    {
        Task<Group> Create(string name, IEnumerable<User> userList, bool defaultGroup = false, bool replaceDefault = false, Guid domainId = new Guid());
        Task Delete(Guid groupId);
        Task AddPolicy(Guid groupId, Guid policyId);
        Task RemovePolicy(Guid groupId, Guid policyId);
        Task AddUsers(Guid groupId, IEnumerable<Guid> userIdList);
        Task RemoveUsers(Guid groupId, IEnumerable<Guid> userIdList);
    }

    public sealed class GroupService : IGroupService
    {
        /// <summary>
        /// Create a new Group
        /// </summary>
        /// <param name="name">Name of the group</param>
        /// <param name="userList">Users whom assigned to the group after creation</param>
        /// <param name="defaultGroup">Is the group should be default (automatically assigned to new users)</param>
        /// <param name="replaceDefault">Should the new Group replace the default Group</param>
        /// <param name="domainId">The Id of the domain the group is in. Leave default value with Single domain settings.</param>
        /// <returns>The new Group instance</returns>
        public async Task<Group> Create(string name, IEnumerable<User> userList, bool defaultGroup = false, bool replaceDefault = false, Guid domainId = new Guid())
        {
            IAuthorityContext context = AuthorityContextProvider.Create();

            if (domainId == Guid.Empty)
            {
                domainId = Common.GetDomainId();
            }

            CreateGroup create = new CreateGroup(context, domainId, name, defaultGroup, replaceDefault, userList.ToList());
            Group result = await create.Do();
            await create.CommitAsync();

            return result;
        }

        /// <summary>
        /// Delete a Group
        /// </summary>
        /// <param name="groupId">Id of the Group to delete</param>
        /// <returns>None</returns>
        public async Task Delete(Guid groupId)
        {
            IAuthorityContext context = AuthorityContextProvider.Create();

            DeleteGroup delete = new DeleteGroup(context, groupId);
            await delete.Do();
            await delete.CommitAsync();
        }

        /// <summary>
        /// Add a Policy to a Group
        /// </summary>
        /// <param name="groupId">Id of the Group</param>
        /// <param name="policyId">Id of the Policy</param>
        /// <returns>None</returns>
        public async Task AddPolicy(Guid groupId, Guid policyId)
        {
            IAuthorityContext context = AuthorityContextProvider.Create();

            AddPolicyToGroup addPolicy = new AddPolicyToGroup(context, groupId, policyId);
            await addPolicy.Do();
            await addPolicy.CommitAsync();
        }

        /// <summary>
        /// Remove a Policy from a Group
        /// </summary>
        /// <param name="groupId">Id of the Group</param>
        /// <param name="policyId">Id of the Policy</param>
        /// <returns>None</returns>
        public async Task RemovePolicy(Guid groupId, Guid policyId)
        {
            IAuthorityContext context = AuthorityContextProvider.Create();

            RemovePolicyFromGroup remove = new RemovePolicyFromGroup(context, groupId, policyId);
            await remove.Do();
            await remove.CommitAsync();
        }

        /// <summary>
        /// Add Users to a Group
        /// </summary>
        /// <param name="groupId">Id of the Group</param>
        /// <param name="userIdList">List of Ids of Users to add to the Group</param>
        /// <returns>None</returns>
        public async Task AddUsers(Guid groupId, IEnumerable<Guid> userIdList)
        {
            IAuthorityContext context = AuthorityContextProvider.Create();

            AddUsersToGroup addUsers = new AddUsersToGroup(context, groupId, userIdList);
            await addUsers.Do();
            await addUsers.CommitAsync();
        }

        /// <summary>
        /// Remove Users from a Group
        /// </summary>
        /// <param name="groupId">Id of the Group</param>
        /// <param name="userIdList">List of Ids of Users whom removed from the Group</param>
        /// <returns>None</returns>
        public async Task RemoveUsers(Guid groupId, IEnumerable<Guid> userIdList)
        {
            IAuthorityContext context = AuthorityContextProvider.Create();

            RemoveUsersFromGroup removeUsers = new RemoveUsersFromGroup(context, groupId, userIdList);
            await removeUsers.Do();
            await removeUsers.CommitAsync();
        }
    }
}
