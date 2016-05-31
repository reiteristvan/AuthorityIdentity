using System.Data.Entity.ModelConfiguration;
using Authority.DomainModel;

namespace Authority.EntityFramework.Configurations
{
    public sealed class ClaimConfiguration : EntityTypeConfiguration<Claim>
    {
        public ClaimConfiguration()
        {
            ToTable("Authority.Claims");

            HasKey(e => e.Id);

            Property(c => c.FriendlyName).IsRequired().HasMaxLength(128);
            Property(c => c.Issuer).IsRequired().HasMaxLength(256);
            Property(c => c.Type).IsRequired().HasMaxLength(256);
            Property(c => c.Value).IsRequired().HasMaxLength(512);
        }
    }
}
