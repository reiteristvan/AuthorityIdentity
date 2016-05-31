using System;

namespace Authority.DomainModel
{
    public class ProductStyle : EntityBase
    {
        public ProductStyle(Guid productId)
        {
            Id = productId;
        }

        public byte[] Logo { get; set; }

        public virtual Product Product { get; set; }
    }
}
