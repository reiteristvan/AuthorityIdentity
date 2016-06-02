namespace Authority.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveClientApplication : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Authority.ClientApplications", "Product_Id", "Authority.Products");
            DropForeignKey("Authority.ClientApplications", "Developer_Id", "Authority.Developers");
            DropIndex("Authority.ClientApplications", new[] { "Product_Id" });
            DropIndex("Authority.ClientApplications", new[] { "Developer_Id" });
            DropTable("Authority.ClientApplications");
        }
        
        public override void Down()
        {
            CreateTable(
                "Authority.ClientApplications",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 128),
                        IsEnabled = c.Boolean(nullable: false),
                        ApplicationId = c.String(nullable: false, maxLength: 128),
                        ApplicationSecret = c.String(nullable: false, maxLength: 128),
                        RedirectUrl = c.String(nullable: false, maxLength: 128),
                        IsPublic = c.Boolean(nullable: false),
                        Product_Id = c.Guid(nullable: false),
                        Developer_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("Authority.ClientApplications", "Developer_Id");
            CreateIndex("Authority.ClientApplications", "Product_Id");
            AddForeignKey("Authority.ClientApplications", "Developer_Id", "Authority.Developers", "Id");
            AddForeignKey("Authority.ClientApplications", "Product_Id", "Authority.Products", "Id");
        }
    }
}
