using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Authority.DomainModel;
using Authority.EntityFramework;

namespace Authority.Operations.Developers
{
    public sealed class DeveloperActivation : OperationWithNoReturnAsync
    {
        private readonly Guid _activationCode;

        public DeveloperActivation(IAuthorityContext authorityContext, Guid activationCode)
            : base(authorityContext)
        {
            _activationCode = activationCode;
        }

        public override async Task Do()
        {
            if (_activationCode == Guid.Empty)
            {
                throw new RequirementFailedException(DevelopersErrorCodes.FailedActivation);
            }

            Developer developer = await Context.Developers
                .FirstOrDefaultAsync(d => d.PendingRegistrationId == _activationCode);

            if (developer == null || developer.IsPending == false || developer.PendingRegistrationId != _activationCode)
            {
                throw new RequirementFailedException(DevelopersErrorCodes.FailedActivation);
            }

            developer.IsPending = false;
            developer.PendingRegistrationId = Guid.Empty;

            Context.Developers.Attach(developer);
            Context.Entry(developer).State = EntityState.Modified;
        }
    }
}
