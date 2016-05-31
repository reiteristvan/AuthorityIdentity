using System.Data.Entity.ModelConfiguration;
using Authority.DomainModel;

namespace Authority.EntityFramework.Configurations
{
    public sealed class ClientApplicationConfiguration : EntityTypeConfiguration<ClientApplication>
    {
        public ClientApplicationConfiguration()
        {
            ToTable("Authority.ClientApplications");

            HasKey(e => e.Id);

            Property(c => c.Name).IsRequired().HasMaxLength(128);
            Property(c => c.ApplicationId).IsRequired().HasMaxLength(128);
            Property(c => c.ApplicationSecret).IsRequired().HasMaxLength(128);
            Property(c => c.RedirectUrl).IsRequired().HasMaxLength(128);
            Property(c => c.IsEnabled).IsRequired();
            Property(c => c.IsPublic).IsRequired();
            
            HasRequired(c => c.Product);
        }
    }
}
