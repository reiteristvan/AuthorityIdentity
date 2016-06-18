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
        private readonly string _siteUrl;
        private readonly string _notificationEmail;
        private readonly string _activationUrl;

        public CreateProduct(IAuthorityContext AuthorityContext,  
                             string name, 
                             string siteUrl, 
                             string notificationEmail,
                             string activationUrl)
            : base(AuthorityContext)
        {
            _name = name;
            _siteUrl = siteUrl;
            _notificationEmail = notificationEmail;
            _activationUrl = activationUrl;
        }

        public override async Task<Guid> Do()
        {
            await Check(() => IsNameAvailable(_name), ProductErrorCodes.NameNotAvailable);

            Product product = new Product
            {
                Name = _name,
                IsActive = true,
                IsPublic = false,
                SiteUrl = _siteUrl,
                NotificationEmail = _notificationEmail,
                ActivationUrl = _activationUrl,
                ApiKey= Guid.NewGuid()
            };

            product.Style = new ProductStyle(product.Id)
            {
                Logo = new byte[10]
            };

            Context.ProductStyles.Add(product.Style);
            Context.Products.Add(product);

            return product.Id;
        }

        private async Task<bool> IsNameAvailable(string name)
        {
            Product product = await Context.Products.FirstOrDefaultAsync(p => p.Name == name);
            return product == null;
        }
    }
}
