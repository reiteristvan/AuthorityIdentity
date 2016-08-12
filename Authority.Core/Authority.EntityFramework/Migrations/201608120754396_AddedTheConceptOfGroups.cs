namespace Authority.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedTheConceptOfGroups : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Authority.Groups",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 256),
                        UserCount = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        Default = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserGroups",
                c => new
                    {
                        User_DomainId = c.Guid(nullable: false),
                        User_Email = c.String(nullable: false, maxLength: 128),
                        Group_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_DomainId, t.User_Email, t.Group_Id })
                .ForeignKey("Authority.Users", t => new { t.User_DomainId, t.User_Email }, cascadeDelete: true)
                .ForeignKey("Authority.Groups", t => t.Group_Id, cascadeDelete: true)
                .Index(t => new { t.User_DomainId, t.User_Email })
                .Index(t => t.Group_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserGroups", "Group_Id", "Authority.Groups");
            DropForeignKey("dbo.UserGroups", new[] { "User_DomainId", "User_Email" }, "Authority.Users");
            DropIndex("dbo.UserGroups", new[] { "Group_Id" });
            DropIndex("dbo.UserGroups", new[] { "User_DomainId", "User_Email" });
            DropTable("dbo.UserGroups");
            DropTable("Authority.Groups");
        }
    }
}
