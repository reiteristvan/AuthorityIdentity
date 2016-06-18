namespace Authority.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveDeveloperEntity : DbMigration
    {
        public override void Up()
        {
            DropColumn("Authority.Products", "OwnerId");
            DropTable("Authority.Developers");
        }
        
        public override void Down()
        {
            CreateTable(
                "Authority.Developers",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Email = c.String(nullable: false, maxLength: 128),
                        DisplayName = c.String(nullable: false, maxLength: 64),
                        PasswordHash = c.String(nullable: false, maxLength: 128),
                        Salt = c.String(nullable: false, maxLength: 128),
                        IsPending = c.Boolean(nullable: false),
                        PendingRegistrationId = c.Guid(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        Created = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("Authority.Products", "OwnerId", c => c.Guid(nullable: false));
        }
    }
}
