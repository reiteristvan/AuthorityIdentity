using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Authority.DomainModel;
using Authority.EntityFramework;

namespace Authority.Operations.Account
{
    public sealed class DeleteUser : OperationWithNoReturnAsync
    {
        private readonly Guid _domainId;
        private readonly string _email;

        public DeleteUser(IAuthorityContext authorityContext, Guid domainId, string email) 
            : base(authorityContext)
        {
            _domainId = domainId;
            _email = email;
        }

        public override async Task Do()
        {
            User user = await Context.Users.FirstOrDefaultAsync(u => u.DomainId == _domainId && u.Email == _email);

            if (user == null)
            {
                throw new RequirementFailedException(AccountErrorCodes.UserNotFound);
            }

            Context.Users.Remove(user);
        }
    }
}
