namespace Authority.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserPolicyManyToMany : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Policies", "User_Id", "dbo.Users");
            DropIndex("dbo.Policies", new[] { "User_Id" });
            CreateTable(
                "dbo.UserPolicies",
                c => new
                    {
                        User_Id = c.Guid(nullable: false),
                        Policy_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_Id, t.Policy_Id })
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .ForeignKey("dbo.Policies", t => t.Policy_Id, cascadeDelete: true)
                .Index(t => t.User_Id)
                .Index(t => t.Policy_Id);
            
            DropColumn("dbo.Policies", "User_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Policies", "User_Id", c => c.Guid());
            DropForeignKey("dbo.UserPolicies", "Policy_Id", "dbo.Policies");
            DropForeignKey("dbo.UserPolicies", "User_Id", "dbo.Users");
            DropIndex("dbo.UserPolicies", new[] { "Policy_Id" });
            DropIndex("dbo.UserPolicies", new[] { "User_Id" });
            DropTable("dbo.UserPolicies");
            CreateIndex("dbo.Policies", "User_Id");
            AddForeignKey("dbo.Policies", "User_Id", "dbo.Users", "Id");
        }
    }
}
