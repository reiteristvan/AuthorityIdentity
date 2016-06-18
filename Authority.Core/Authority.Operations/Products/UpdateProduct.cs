using System;
using System.Threading.Tasks;
using Authority.DomainModel;
using Authority.EntityFramework;

namespace Authority.Operations.Products
{
    public sealed class UpdateProduct : OperationWithReturnValueAsync<Product>
    {
        private readonly string _name;
        private readonly bool _isPublic;
        private readonly string _siteUrl;
        private readonly string _landingPage;
        private readonly bool _isActive;

        public UpdateProduct(IAuthorityContext AuthorityContext, 
            string name, bool isActive, bool isPublic, string siteUrl, string landingPage)
            : base(AuthorityContext)
        {
            _name = name;
            _isPublic = isPublic;
            _siteUrl = siteUrl;
            _landingPage = landingPage;
            _isActive = isActive;
        }

        public override Task<Product> Do()
        {
            throw new NotImplementedException();
        }
    }
}
