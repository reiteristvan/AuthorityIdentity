namespace Authority.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserCompositeKey : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Users", "ProductId", "dbo.Products");
            DropForeignKey("dbo.UserPolicies", "User_Id", "dbo.Users");
            DropIndex("dbo.Users", new[] { "ProductId" });
            DropIndex("dbo.UserPolicies", new[] { "User_Id" });
            RenameColumn(table: "dbo.UserPolicies", name: "User_Id", newName: "User_ProductId");
            DropPrimaryKey("dbo.Users");
            DropPrimaryKey("dbo.UserPolicies");
            AddColumn("dbo.UserPolicies", "User_Email", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.Users", new[] { "ProductId", "Email" });
            AddPrimaryKey("dbo.UserPolicies", new[] { "User_ProductId", "User_Email", "Policy_Id" });
            CreateIndex("dbo.UserPolicies", new[] { "User_ProductId", "User_Email" });
            AddForeignKey("dbo.UserPolicies", new[] { "User_ProductId", "User_Email" }, "dbo.Users", new[] { "ProductId", "Email" }, cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserPolicies", new[] { "User_ProductId", "User_Email" }, "dbo.Users");
            DropIndex("dbo.UserPolicies", new[] { "User_ProductId", "User_Email" });
            DropPrimaryKey("dbo.UserPolicies");
            DropPrimaryKey("dbo.Users");
            DropColumn("dbo.UserPolicies", "User_Email");
            AddPrimaryKey("dbo.UserPolicies", new[] { "User_Id", "Policy_Id" });
            AddPrimaryKey("dbo.Users", "Id");
            RenameColumn(table: "dbo.UserPolicies", name: "User_ProductId", newName: "User_Id");
            CreateIndex("dbo.UserPolicies", "User_Id");
            CreateIndex("dbo.Users", "ProductId");
            AddForeignKey("dbo.UserPolicies", "User_Id", "dbo.Users", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Users", "ProductId", "dbo.Products", "Id");
        }
    }
}
