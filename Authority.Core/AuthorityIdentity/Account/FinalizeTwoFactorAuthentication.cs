using System;
using System.Data.Entity;
using System.Threading.Tasks;
using AuthorityIdentity.DomainModel;
using AuthorityIdentity.EntityFramework;

namespace AuthorityIdentity.Account
{
    public sealed class FinalizeTwoFactorAuthentication : OperationWithReturnValueAsync<bool>
    {
        private readonly Guid _userId;
        private readonly string _token;

        public FinalizeTwoFactorAuthentication(IAuthorityContext authorityContext, Guid userId, string token)
            : base(authorityContext)
        {
            _userId = userId;
            _token = token;
        }

        public override async Task<bool> Do()
        {
            User user = await Context.Users.FirstOrDefaultAsync(u => u.Id == _userId);

            Require(() => user != null, ErrorCodes.UserNotFound);

            bool result = user.TwoFactorToken == _token;
            user.TwoFactorToken = ""; // it should be cleared anyway

            return result;
        }
    }
}
