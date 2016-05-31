namespace Authority.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Seed : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Claims",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Issuer = c.String(nullable: false, maxLength: 256),
                        Type = c.String(nullable: false, maxLength: 256),
                        Value = c.String(nullable: false, maxLength: 512),
                        Policy_Id = c.Guid(),
                        User_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Policies", t => t.Policy_Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.Policy_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.ClientApplications",
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.Product_Id)
                .ForeignKey("dbo.Developers", t => t.Developer_Id)
                .Index(t => t.Product_Id)
                .Index(t => t.Developer_Id);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 128),
                        IsPublic = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Policies",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        Product_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.Product_Id)
                .Index(t => t.Product_Id);
            
            CreateTable(
                "dbo.Developers",
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
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Email = c.String(nullable: false, maxLength: 128),
                        Username = c.String(nullable: false, maxLength: 64),
                        PasswordHash = c.String(nullable: false, maxLength: 128),
                        Salt = c.String(nullable: false, maxLength: 128),
                        IsPending = c.Boolean(nullable: false),
                        PendingRegistrationId = c.Guid(nullable: false),
                        IsExternal = c.Boolean(nullable: false),
                        ExternalProviderName = c.String(),
                        ExternalToken = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Claims", "User_Id", "dbo.Users");
            DropForeignKey("dbo.ClientApplications", "Developer_Id", "dbo.Developers");
            DropForeignKey("dbo.ClientApplications", "Product_Id", "dbo.Products");
            DropForeignKey("dbo.Policies", "Product_Id", "dbo.Products");
            DropForeignKey("dbo.Claims", "Policy_Id", "dbo.Policies");
            DropIndex("dbo.Policies", new[] { "Product_Id" });
            DropIndex("dbo.ClientApplications", new[] { "Developer_Id" });
            DropIndex("dbo.ClientApplications", new[] { "Product_Id" });
            DropIndex("dbo.Claims", new[] { "User_Id" });
            DropIndex("dbo.Claims", new[] { "Policy_Id" });
            DropTable("dbo.Users");
            DropTable("dbo.Developers");
            DropTable("dbo.Policies");
            DropTable("dbo.Products");
            DropTable("dbo.ClientApplications");
            DropTable("dbo.Claims");
        }
    }
}
