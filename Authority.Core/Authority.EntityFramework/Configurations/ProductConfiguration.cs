using System.Data.Entity.ModelConfiguration;
using Authority.DomainModel;

namespace Authority.EntityFramework.Configurations
{
    public sealed class ProductConfiguration : EntityTypeConfiguration<Product>
    {
        public ProductConfiguration()
        {
            ToTable("Authority.Products");

            HasKey(e => e.Id);

            Property(p => p.OwnerId).IsRequired();
            Property(p => p.Name).IsRequired().HasMaxLength(128);
            Property(p => p.IsPublic).IsRequired();
            Property(p => p.IsActive).IsRequired();
            Property(p => p.SiteUrl).HasMaxLength(128).IsRequired();
            Property(p => p.NotificationEmail).HasMaxLength(128).IsRequired();
            Property(p => p.ActivationUrl).HasMaxLength(128).IsRequired();
            Property(p => p.ApiKey).IsRequired();

            HasMany(p => p.Policies);
            HasMany(p => p.Claims).WithOptional().WillCascadeOnDelete(false);
            HasOptional(p => p.Style).WithRequired(ps => ps.Product).WillCascadeOnDelete(true);
        }
    }
}
