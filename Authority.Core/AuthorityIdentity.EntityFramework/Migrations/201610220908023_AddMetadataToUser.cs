namespace AuthorityIdentity.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMetadataToUser : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Authority.Metadata",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Data = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddForeignKey("Authority.Users", "Id", "Authority.Metadata", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("Authority.Users", "Id", "Authority.Metadata");
            DropTable("Authority.Metadata");
        }
    }
}
