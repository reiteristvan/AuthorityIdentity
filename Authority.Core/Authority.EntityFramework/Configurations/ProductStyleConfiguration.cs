using System.Data.Entity.ModelConfiguration;
using Authority.DomainModel;

namespace IdentityServer.EntityFramework.Configurations
{
    public sealed class ProductStyleConfiguration : EntityTypeConfiguration<ProductStyle>
    {
        public ProductStyleConfiguration()
        {
            ToTable("Authority.ProductStyles");

            HasKey(e => e.Id);

            Property(p => p.Logo).IsRequired();
        }
    }
}
