using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Authority.DomainModel;
using Authority.EntityFramework;

namespace Authority.Operations.Products
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
            await Check(() => IsNameAvailable(_name), DomainErrorCodes.NameNotAvailable);

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
