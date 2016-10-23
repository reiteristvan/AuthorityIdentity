using System.Data.Entity.ModelConfiguration;
using AuthorityIdentity.DomainModel;

namespace AuthorityIdentity.EntityFramework.Configurations
{
    public sealed class MetadataConfiguration : EntityTypeConfiguration<Metadata>
    {
        public MetadataConfiguration()
        {
            ToTable(Metadata.TableName);

            Property(m => m.Data).IsRequired();
        }
    }
}
