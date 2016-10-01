using System;
using System.Data.Entity;
using System.Threading.Tasks;
using AuthorityIdentity.DomainModel;
using AuthorityIdentity.EntityFramework;

namespace AuthorityIdentity.Account
{
    public sealed class SetUserStatus : OperationWithNoReturnAsync
    {
        private readonly Guid _userId;
        private readonly bool _isActive;

        public SetUserStatus(IAuthorityContext authorityContext, Guid userId, bool isActive) 
            : base(authorityContext)
        {
            _userId = userId;
            _isActive = isActive;
        }

        public override async Task Do()
        {
            User user = await Context.Users.FirstOrDefaultAsync(u => u.Id == _userId);

            Require(() => user != null, ErrorCodes.UserNotFound);

            user.IsActive = _isActive;
        }
    }
}
