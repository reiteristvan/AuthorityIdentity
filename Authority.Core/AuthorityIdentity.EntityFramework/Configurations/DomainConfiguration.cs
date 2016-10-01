using System.Data.Entity.ModelConfiguration;
using AuthorityIdentity.DomainModel;

namespace AuthorityIdentity.EntityFramework.Configurations
{
    public sealed class DomainConfiguration : EntityTypeConfiguration<Domain>
    {
        public DomainConfiguration()
        {
            ToTable(Domain.TableName);

            HasKey(e => e.Id);

            Property(d => d.Name).IsRequired().HasMaxLength(128);
            Property(d => d.IsActive).IsRequired();

            HasMany(d => d.Groups).WithRequired().WillCascadeOnDelete(true);
            HasMany(d => d.Policies).WithRequired().WillCascadeOnDelete(true);
            HasMany(d => d.Claims).WithOptional().WillCascadeOnDelete(true);
        }
    }
}
