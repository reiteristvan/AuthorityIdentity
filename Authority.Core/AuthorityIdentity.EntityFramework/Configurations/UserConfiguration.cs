using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using AuthorityIdentity.DomainModel;

namespace AuthorityIdentity.EntityFramework.Configurations
{
    public sealed class UserConfiguration : EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
            ToTable(User.TableName);

            HasKey(e => new { e.DomainId, e.Email });

            Property(u => u.Id)
                .IsRequired()
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("IX_Authority.Users_Id")));

            Property(u => u.DomainId).IsRequired();
            Property(u => u.Email).IsRequired().HasMaxLength(128);
            Property(u => u.Username).IsRequired().HasMaxLength(64);
            Property(u => u.LastLogin).IsRequired();
            Property(u => u.PasswordHash).IsRequired().HasMaxLength(128);
            Property(u => u.Salt).IsRequired().HasMaxLength(128);
            Property(u => u.IsPending).IsRequired();
            Property(u => u.IsActive).IsRequired();
            Property(u => u.PendingRegistrationId).IsRequired();
            Property(u => u.IsTwoFactorEnabled).IsRequired();
            Property(u => u.TwoFactorToken).IsRequired();

            HasMany(u => u.Groups).WithMany(g => g.Users);
            HasMany(u => u.Policies).WithMany(p => p.Users);
        }
    }
}
