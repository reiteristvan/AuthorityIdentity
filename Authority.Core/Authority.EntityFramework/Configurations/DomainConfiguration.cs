using System.Data.Entity.ModelConfiguration;
using Authority.DomainModel;

namespace Authority.EntityFramework.Configurations
{
    public sealed class DomainConfiguration : EntityTypeConfiguration<Domain>
    {
        public DomainConfiguration()
        {
            ToTable("Authority.Domains");

            HasKey(e => e.Id);

            Property(p => p.Name).IsRequired().HasMaxLength(128);
            Property(p => p.IsActive).IsRequired();

            HasMany(p => p.Policies).WithRequired().WillCascadeOnDelete(true);
            HasMany(p => p.Claims).WithOptional().WillCascadeOnDelete(true);
        }
    }
}
