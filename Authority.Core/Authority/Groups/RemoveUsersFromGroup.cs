using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Authority.DomainModel;
using Authority.EntityFramework;

namespace Authority.Groups
{
    public sealed class RemoveUsersFromGroup : OperationWithNoReturnAsync
    {
        private readonly Guid _groupId;
        private readonly IEnumerable<Guid> _userIdList;

        public RemoveUsersFromGroup(IAuthorityContext authorityContext, Guid groupId, IEnumerable<Guid> userIdList)
            : base(authorityContext)
        {
            _groupId = groupId;
            _userIdList = userIdList;
        }

        public override async Task Do()
        {
            Group group = await Context.Groups
                .Include(g => g.Users)
                .FirstOrDefaultAsync(g => g.Id == _groupId);

            Require(() => group != null, ErrorCodes.GroupNotFound);

            List<User> users = Context.Users.Where(u => _userIdList.Contains(u.Id)).ToList();

            Require(() => users.All(u => u.DomainId == group.DomainId), ErrorCodes.DomainMismatch);

            foreach (User user in users.Where(u => group.Users.Any(gu => gu.Id != u.Id)))
            {
                group.Users.Remove(user);
            }
        }
    }
}
