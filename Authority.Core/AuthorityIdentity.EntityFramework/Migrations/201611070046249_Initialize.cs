namespace AuthorityIdentity.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initialize : DbMigration
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
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Authority.Domains", t => t.Domain_Id, cascadeDelete: true)
                .Index(t => t.Domain_Id);
            
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
                .ForeignKey("Authority.Domains", t => t.DomainId, cascadeDelete: true)
                .Index(t => t.DomainId);
            
            CreateTable(
                "Authority.Groups",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        DomainId = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 256),
                        Default = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Authority.Domains", t => t.DomainId, cascadeDelete: true)
                .Index(t => t.DomainId);
            
            CreateTable(
                "Authority.Users",
                c => new
                    {
                        DomainId = c.Guid(nullable: false),
                        Email = c.String(nullable: false, maxLength: 128),
                        Username = c.String(nullable: false, maxLength: 64),
                        LastLogin = c.DateTimeOffset(nullable: false, precision: 7),
                        PasswordHash = c.String(nullable: false, maxLength: 128),
                        Salt = c.String(nullable: false, maxLength: 128),
                        IsPending = c.Boolean(nullable: false),
                        PendingRegistrationId = c.Guid(nullable: false),
                        IsExternal = c.Boolean(nullable: false),
                        ExternalProviderName = c.String(),
                        ExternalToken = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        IsTwoFactorEnabled = c.Boolean(nullable: false),
                        TwoFactorToken = c.String(nullable: false),
                        TwoFactorType = c.Int(nullable: false),
                        TwoFactorTarget = c.String(nullable: false),
                        Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.DomainId, t.Email })
                .ForeignKey("Authority.Metadata", t => t.Id)
                .Index(t => t.Id, name: "IX_Authority.Users_Id");
            
            CreateTable(
                "Authority.Metadata",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Data = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
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
                "Authority.Invites",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Email = c.String(nullable: false, maxLength: 256),
                        DomainId = c.Guid(nullable: false),
                        Created = c.DateTimeOffset(nullable: false, precision: 7),
                        Expire = c.DateTimeOffset(precision: 7),
                        Accepted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "Authority.PolicyAuthorityClaims",
                c => new
                    {
                        Policy_Id = c.Guid(nullable: false),
                        AuthorityClaim_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Policy_Id, t.AuthorityClaim_Id })
                .ForeignKey("Authority.Policies", t => t.Policy_Id, cascadeDelete: false)
                .ForeignKey("Authority.Claims", t => t.AuthorityClaim_Id, cascadeDelete: false)
                .Index(t => t.Policy_Id)
                .Index(t => t.AuthorityClaim_Id);
            
            CreateTable(
                "Authority.GroupPolicies",
                c => new
                    {
                        Group_Id = c.Guid(nullable: false),
                        Policy_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Group_Id, t.Policy_Id })
                .ForeignKey("Authority.Groups", t => t.Group_Id, cascadeDelete: true)
                .ForeignKey("Authority.Policies", t => t.Policy_Id, cascadeDelete: false)
                .Index(t => t.Group_Id)
                .Index(t => t.Policy_Id);
            
            CreateTable(
                "Authority.UserGroups",
                c => new
                    {
                        User_DomainId = c.Guid(nullable: false),
                        User_Email = c.String(nullable: false, maxLength: 128),
                        Group_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_DomainId, t.User_Email, t.Group_Id })
                .ForeignKey("Authority.Users", t => new { t.User_DomainId, t.User_Email }, cascadeDelete: false)
                .ForeignKey("Authority.Groups", t => t.Group_Id, cascadeDelete: true)
                .Index(t => new { t.User_DomainId, t.User_Email })
                .Index(t => t.Group_Id);
            
            CreateTable(
                "Authority.UserPolicies",
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
            DropForeignKey("Authority.Groups", "DomainId", "Authority.Domains");
            DropForeignKey("Authority.Claims", "Domain_Id", "Authority.Domains");
            DropForeignKey("Authority.UserPolicies", "Policy_Id", "Authority.Policies");
            DropForeignKey("Authority.UserPolicies", new[] { "User_DomainId", "User_Email" }, "Authority.Users");
            DropForeignKey("Authority.Users", "Id", "Authority.Metadata");
            DropForeignKey("Authority.UserGroups", "Group_Id", "Authority.Groups");
            DropForeignKey("Authority.UserGroups", new[] { "User_DomainId", "User_Email" }, "Authority.Users");
            DropForeignKey("Authority.GroupPolicies", "Policy_Id", "Authority.Policies");
            DropForeignKey("Authority.GroupPolicies", "Group_Id", "Authority.Groups");
            DropForeignKey("Authority.PolicyAuthorityClaims", "AuthorityClaim_Id", "Authority.Claims");
            DropForeignKey("Authority.PolicyAuthorityClaims", "Policy_Id", "Authority.Policies");
            DropIndex("Authority.UserPolicies", new[] { "Policy_Id" });
            DropIndex("Authority.UserPolicies", new[] { "User_DomainId", "User_Email" });
            DropIndex("Authority.UserGroups", new[] { "Group_Id" });
            DropIndex("Authority.UserGroups", new[] { "User_DomainId", "User_Email" });
            DropIndex("Authority.GroupPolicies", new[] { "Policy_Id" });
            DropIndex("Authority.GroupPolicies", new[] { "Group_Id" });
            DropIndex("Authority.PolicyAuthorityClaims", new[] { "AuthorityClaim_Id" });
            DropIndex("Authority.PolicyAuthorityClaims", new[] { "Policy_Id" });
            DropIndex("Authority.Users", "IX_Authority.Users_Id");
            DropIndex("Authority.Groups", new[] { "DomainId" });
            DropIndex("Authority.Policies", new[] { "DomainId" });
            DropIndex("Authority.Claims", new[] { "Domain_Id" });
            DropTable("Authority.UserPolicies");
            DropTable("Authority.UserGroups");
            DropTable("Authority.GroupPolicies");
            DropTable("Authority.PolicyAuthorityClaims");
            DropTable("Authority.Invites");
            DropTable("Authority.Domains");
            DropTable("Authority.Metadata");
            DropTable("Authority.Users");
            DropTable("Authority.Groups");
            DropTable("Authority.Policies");
            DropTable("Authority.Claims");
        }
    }
}
