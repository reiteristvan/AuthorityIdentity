using System;
using System.Data.Entity;
using System.Threading.Tasks;
using AuthorityIdentity.DomainModel;
using AuthorityIdentity.EntityFramework;

namespace AuthorityIdentity.Account
{
    public sealed class SetMetadata : OperationWithNoReturnAsync
    {
        private readonly Guid _userId;
        private readonly string _metadata;

        public SetMetadata(IAuthorityContext context, Guid userId, string metadata)
            : base(context)
        {
            _userId = userId;
            _metadata = metadata;
        }

        public override async Task Do()
        {
            User user = await Context.Users.FirstOrDefaultAsync(u => u.Id == _userId);

            Require(() => user != null, ErrorCodes.UserNotFound);

            user.Metadata = _metadata;
        }
    }
}
