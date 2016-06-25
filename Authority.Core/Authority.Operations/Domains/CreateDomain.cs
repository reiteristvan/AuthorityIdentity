using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Authority.DomainModel;
using Authority.EntityFramework;

namespace Authority.Operations.Products
{
    public sealed class CreateProduct : OperationWithReturnValueAsync<Guid>
    {
        private readonly string _name;

        public CreateProduct(IAuthorityContext AuthorityContext, string name)
            : base(AuthorityContext)
        {
            _name = name;
        }

        public override async Task<Guid> Do()
        {
            await Check(() => IsNameAvailable(_name), ProductErrorCodes.NameNotAvailable);

            Domain product = new Domain
            {
                Name = _name,
                IsActive = true
            };

            Context.Domains.Add(product);

            return product.Id;
        }

        private async Task<bool> IsNameAvailable(string name)
        {
            Domain product = await Context.Domains.FirstOrDefaultAsync(p => p.Name == name);
            return product == null;
        }
    }
}
