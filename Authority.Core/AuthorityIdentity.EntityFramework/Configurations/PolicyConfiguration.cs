using System.Data.Entity.ModelConfiguration;
using AuthorityIdentity.DomainModel;

namespace AuthorityIdentity.EntityFramework.Configurations
{
    public sealed class PolicyConfiguration : EntityTypeConfiguration<Policy>
    {
        public PolicyConfiguration()
        {
            ToTable(Policy.TableName);

            HasKey(e => e.Id);

            Property(p => p.DomainId).IsRequired();
            Property(p => p.Name).IsRequired();
            Property(p => p.Default).IsRequired();

            HasMany(p => p.Claims).WithMany(c => c.Policies);
            HasMany(p => p.Users);
        }
    }
}
