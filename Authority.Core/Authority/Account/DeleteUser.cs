using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Authority.DomainModel;
using Authority.EntityFramework;

namespace Authority.Account
{
    public sealed class DeleteUser : OperationWithNoReturnAsync
    {
        private readonly Guid _userId;

        public DeleteUser(IAuthorityContext authorityContext, Guid userId) 
            : base(authorityContext)
        {
            _userId = userId;
        }

        public override async Task Do()
        {
            User user = await Context.Users.FirstOrDefaultAsync(u => u.Id == _userId);

            Require(() => user != null, ErrorCodes.UserNotFound);

            Context.Users.Remove(user);
        }
    }
}
