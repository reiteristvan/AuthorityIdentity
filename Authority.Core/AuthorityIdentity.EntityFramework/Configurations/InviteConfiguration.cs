using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using AuthorityIdentity.DomainModel;

namespace AuthorityIdentity.EntityFramework.Configurations
{
    public sealed class InviteConfiguration : EntityTypeConfiguration<Invite>
    {
        public InviteConfiguration()
        {
            ToTable(Invite.TableName);

            HasKey(e => e.Id);

            Property(i => i.DomainId)
                .IsRequired()
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("IX_Authority.Invites_DomainId")));

            Property(i => i.Email).HasMaxLength(256).IsRequired();
            Property(i => i.DomainId).IsRequired();
            Property(i => i.Created).IsRequired();
            Property(i => i.Expire).IsOptional();
            Property(i => i.Accepted).IsRequired();
        }
    }
}
