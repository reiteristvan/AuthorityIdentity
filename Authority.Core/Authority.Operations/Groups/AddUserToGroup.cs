using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Authority.DomainModel;
using Authority.EntityFramework;

namespace Authority.Operations.Groups
{
    public sealed class AddUserToGroup : OperationWithNoReturnAsync
    {
        private readonly Guid _userId;
        private readonly Guid _groupId;

        public AddUserToGroup(IAuthorityContext authorityContext, Guid userId, Guid groupId) 
            : base(authorityContext)
        {
            _userId = userId;
            _groupId = groupId;
        }

        public override async Task Do()
        {
            User user = await Context.Users
                .Include(u => u.Groups)
                .FirstOrDefaultAsync(u => u.Id == _userId);

            Require(() => user != null, GroupErrorCodes.UserNotFound);

            Group group = await Context.Groups.FirstOrDefaultAsync(g => g.Id == _groupId);

            Require(() => group != null, GroupErrorCodes.GroupNotFound);
            Require(() => group.DomainId == user.DomainId, GroupErrorCodes.DomainNotFound);

            if (user.Groups.Any(g => g.Id == group.Id))
            {
                return;
            }

            user.Groups.Add(group);
        }
    }
}
