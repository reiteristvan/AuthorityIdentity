using System.Data.Entity.ModelConfiguration;
using Authority.DomainModel;

namespace Authority.EntityFramework.Configurations
{
    public sealed class InviteConfiguration : EntityTypeConfiguration<Invite>
    {
        public InviteConfiguration()
        {
            ToTable(Invite.TableName);

            HasKey(e => e.Id);

            Property(i => i.Email).HasMaxLength(256).IsRequired();
            Property(i => i.DomainId).IsRequired();
            Property(i => i.Created).IsRequired();
            Property(i => i.Expire).IsOptional();
            Property(i => i.Accepted).IsRequired();
        }
    }
}
