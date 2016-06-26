using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Authority.DomainModel;
using Authority.EntityFramework;

namespace Authority.Operations.Account
{
    public sealed class SetUserStatus : OperationWithNoReturnAsync
    {
        private readonly Guid _domainId;
        private readonly string _email;
        private readonly bool _isActive;

        public SetUserStatus(IAuthorityContext authorityContext, Guid domainId, string email, bool isActive) 
            : base(authorityContext)
        {
            _domainId = domainId;
            _email = email;
            _isActive = isActive;
        }

        public override async Task Do()
        {
            User user = await Context.Users.FirstOrDefaultAsync(u => u.DomainId == _domainId && u.Email == _email);

            if (user == null)
            {
                throw new RequirementFailedException(AccountErrorCodes.UserNotFound);
            }

            user.IsActive = _isActive;
        }
    }
}
