using System.Data.Entity.ModelConfiguration;
using Authority.DomainModel;

namespace Authority.EntityFramework.Configurations
{
    public sealed class GroupConfiguration : EntityTypeConfiguration<Group>
    {
        public GroupConfiguration()
        {
            ToTable(Group.TableName);

            HasKey(e => e.Id);

            Property(g => g.DomainId).IsRequired();
            Property(g => g.Name).IsRequired().HasMaxLength(256);
            Property(g => g.UserCount).IsRequired();
            Property(g => g.Default).IsRequired();
            Property(g => g.IsActive).IsRequired();

            HasMany(g => g.Users);
            HasMany(g => g.Policies).WithMany(p => p.Groups);
        }
    }
}
