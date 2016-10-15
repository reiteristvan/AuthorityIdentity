using System;
using System.Data.Entity;
using System.Threading.Tasks;
using AuthorityIdentity.DomainModel;
using AuthorityIdentity.EntityFramework;

namespace AuthorityIdentity.Account
{
    public sealed class AddTwoFactorAuthenticationModel
    {
        public Guid UserId { get; set; }
        public TwoFactorType TwoFactorType { get; set; }
        public string TwoFactorTarget { get; set; }
    }

    public sealed class AddTwoFactorAuthenticationToUser : OperationWithNoReturnAsync
    {
        private readonly AddTwoFactorAuthenticationModel _model;

        public AddTwoFactorAuthenticationToUser(IAuthorityContext authorityContext, AddTwoFactorAuthenticationModel model) 
            : base(authorityContext)
        {
            _model = model;
        }

        public override async Task Do()
        {
            User user = await Context.Users.FirstOrDefaultAsync(u => u.Id == _model.UserId);

            Require(() => user != null, ErrorCodes.UserNotFound);

            user.IsTwoFactorEnabled = true;
            user.TwoFactorType = _model.TwoFactorType;
            user.TwoFactorTarget = _model.TwoFactorTarget;
        }
    }
}
