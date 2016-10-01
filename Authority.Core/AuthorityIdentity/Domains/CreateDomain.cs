using System;
using System.Data.Entity;
using System.Threading.Tasks;
using AuthorityIdentity.DomainModel;
using AuthorityIdentity.EntityFramework;

namespace AuthorityIdentity.Domains
{
    public sealed class CreateDomain : OperationWithReturnValueAsync<Guid>
    {
        private readonly string _name;

        public CreateDomain(IAuthorityContext AuthorityContext, string name)
            : base(AuthorityContext)
        {
            _name = name;
        }

        public override async Task<Guid> Do()
        {
            await Require(() => IsNameAvailable(_name), ErrorCodes.DomainNameNotAvailable);
            Domain domain = new Domain
            {
                Name = _name,
                IsActive = true
            };

            Context.Domains.Add(domain);
            return domain.Id;
        }

        private async Task<bool> IsNameAvailable(string name)
        {
            Domain domain = await Context.Domains.FirstOrDefaultAsync(p => p.Name == name);
            return domain == null;
        }
    }
}
