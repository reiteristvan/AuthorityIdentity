using System.Data.Entity.ModelConfiguration;
using Authority.DomainModel;

namespace Authority.EntityFramework.Configurations
{
    public sealed class DeveloperConfiguration : EntityTypeConfiguration<Developer>
    {
        public DeveloperConfiguration()
        {
            ToTable("Authority.Developers");

            HasKey(e => e.Id);

            Property(d => d.Email).IsRequired().HasMaxLength(128);
            Property(d => d.DisplayName).IsRequired().HasMaxLength(64);
            Property(d => d.PasswordHash).IsRequired().HasMaxLength(128);
            Property(d => d.Salt).IsRequired().HasMaxLength(128);
            Property(d => d.IsPending).IsRequired();
            Property(d => d.IsActive).IsRequired();
            Property(d => d.PendingRegistrationId).IsRequired();
        }
    }
}
