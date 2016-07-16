using System;
using System.Linq;
using System.Threading.Tasks;
using Authority.DomainModel;
using Authority.EntityFramework;
using Authority.Operations.Observers;

namespace Authority.Operations.Account
{
    public sealed class InviteUser : OperationWithReturnValue<Guid>
    {
        private readonly string _email;
        private readonly Guid _domainId;
        private readonly DateTimeOffset? _expireOn;
        private Invite _invite;

        public InviteUser(IAuthorityContext context, string email, Guid domainId, DateTimeOffset? expireOn = null)
            : base(context)
        {
            _email = email;
            _domainId = domainId;
            _expireOn = expireOn;
        }

        public override Guid Do()
        {
            _invite = new Invite
            {
                Email = _email,
                DomainId = _domainId,
                Created = DateTimeOffset.UtcNow,
                Expire = _expireOn,
                Accepted = false
            };

            Context.Invites.Add(_invite);

            return _invite.Id;
        }

        public override void Commit()
        {
            base.Commit();

            if (Authority.Observers.Any())
            {
                Authority.Observers.ForEach(o => o.OnInvited(new InviteInfo
                {
                    DomainId = _domainId,
                    Email = _email
                }));
            }

            if (Authority.EmailService != null)
            {
                Authority.EmailService.SendInvite(_invite.Id);
            }
        }

        public override async Task CommitAsync()
        {
            await base.CommitAsync();

            if (Authority.Observers.Any())
            {
                Authority.Observers.ForEach(o => o.OnInvited(new InviteInfo
                {
                    DomainId = _domainId,
                    Email = _email
                }));
            }

            if (Authority.EmailService != null)
            {
                await Authority.EmailService.SendInviteAsync(_invite.Id);
            }
        }
    }
}
