namespace Authority.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FreshStart : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Authority.Claims",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FriendlyName = c.String(nullable: false, maxLength: 128),
                        Issuer = c.String(nullable: false, maxLength: 256),
                        Type = c.String(nullable: false, maxLength: 256),
                        Value = c.String(nullable: false, maxLength: 512),
                        Domain_Id = c.Guid(),
                        Policy_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Authority.Domains", t => t.Domain_Id, cascadeDelete: true)
                .ForeignKey("Authority.Policies", t => t.Policy_Id)
                .Index(t => t.Domain_Id)
                .Index(t => t.Policy_Id);
            
            CreateTable(
                "Authority.Domains",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 128),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "Authority.Policies",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        DomainId = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                        Default = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Authority.Domains", t => t.DomainId)
                .Index(t => t.DomainId);
            
            CreateTable(
                "Authority.Users",
                c => new
                    {
                        DomainId = c.Guid(nullable: false),
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
                        Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.DomainId, t.Email });
            
            CreateTable(
                "dbo.UserPolicies",
                c => new
                    {
                        User_DomainId = c.Guid(nullable: false),
                        User_Email = c.String(nullable: false, maxLength: 128),
                        Policy_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_DomainId, t.User_Email, t.Policy_Id })
                .ForeignKey("Authority.Users", t => new { t.User_DomainId, t.User_Email }, cascadeDelete: true)
                .ForeignKey("Authority.Policies", t => t.Policy_Id, cascadeDelete: true)
                .Index(t => new { t.User_DomainId, t.User_Email })
                .Index(t => t.Policy_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Authority.Policies", "DomainId", "Authority.Domains");
            DropForeignKey("dbo.UserPolicies", "Policy_Id", "Authority.Policies");
            DropForeignKey("dbo.UserPolicies", new[] { "User_DomainId", "User_Email" }, "Authority.Users");
            DropForeignKey("Authority.Claims", "Policy_Id", "Authority.Policies");
            DropForeignKey("Authority.Claims", "Domain_Id", "Authority.Domains");
            DropIndex("dbo.UserPolicies", new[] { "Policy_Id" });
            DropIndex("dbo.UserPolicies", new[] { "User_DomainId", "User_Email" });
            DropIndex("Authority.Policies", new[] { "DomainId" });
            DropIndex("Authority.Claims", new[] { "Policy_Id" });
            DropIndex("Authority.Claims", new[] { "Domain_Id" });
            DropTable("dbo.UserPolicies");
            DropTable("Authority.Users");
            DropTable("Authority.Policies");
            DropTable("Authority.Domains");
            DropTable("Authority.Claims");
        }
    }
}
