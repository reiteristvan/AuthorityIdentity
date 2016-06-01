using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Authority.EntityFramework;

namespace Authority.Operations.Products
{
    public sealed class DeleteProduct : Operation
    {
        public DeleteProduct(IAuthorityContext AuthorityContext)
            : base(AuthorityContext)
        {
            
        }

        public async Task Delete(Guid userId, Guid productId)
        {
            
        }
    }
}
