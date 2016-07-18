using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Authority.DomainModel;
using Authority.EntityFramework;

namespace Authority.Operations.Account
{
    public sealed class ActivateUser : OperationWithNoReturnAsync
    {
        private readonly Guid _activationCode;
        private User _user;

        public ActivateUser(IAuthorityContext authorityContext, Guid activationCode)
            : base(authorityContext)
        {
            _activationCode = activationCode;
        }

        public override async Task Do()
        {
            _user = await Context.Users
                .FirstOrDefaultAsync(u => u.PendingRegistrationId == _activationCode);

            Require(() => _user != null && _user.IsPending, AccountErrorCodes.FailedActivation);

            Domain domain = await Context.Domains.FirstOrDefaultAsync(p => p.Id == _user.DomainId);

            Require(() => _activationCode != Guid.Empty && domain != null && domain.IsActive, AccountErrorCodes.FailedActivation);

            _user.PendingRegistrationId = Guid.Empty;
            _user.IsPending = false;
        }

        public override void Commit()
        {
            base.Commit();

            if (Authority.Observers.Any())
            {
                Authority.Observers.ForEach(o => o.OnActivated(_user));
            }
        }

        public override async Task CommitAsync()
        {
            await base.CommitAsync();

            if (Authority.Observers.Any())
            {
                Authority.Observers.ForEach(o => o.OnActivated(_user));
            }
        }
    }
}
