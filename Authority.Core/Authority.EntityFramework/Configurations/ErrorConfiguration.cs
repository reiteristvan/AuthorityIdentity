using System.Data.Entity.ModelConfiguration;
using Authority.DomainModel;

namespace Authority.EntityFramework.Configurations
{
    public sealed class ErrorConfiguration : EntityTypeConfiguration<Error>
    {
        public ErrorConfiguration()
        {
            ToTable("Authority.Errors");

            HasKey(e => e.Id);

            Property(e => e.Type).IsRequired().HasMaxLength(256);
            Property(e => e.StackTrace).IsRequired();
            Property(e => e.Message).IsRequired();
            Property(e => e.Date).IsRequired();
        }
    }
}
