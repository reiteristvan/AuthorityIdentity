namespace Authority.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserPolicyClaimsRefactor : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Claims", "User_Id", "dbo.Users");
            DropIndex("dbo.Claims", new[] { "User_Id" });
            AddColumn("dbo.Policies", "User_Id", c => c.Guid());
            CreateIndex("dbo.Policies", "User_Id");
            AddForeignKey("dbo.Policies", "User_Id", "dbo.Users", "Id");
            DropColumn("dbo.Claims", "User_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Claims", "User_Id", c => c.Guid());
            DropForeignKey("dbo.Policies", "User_Id", "dbo.Users");
            DropIndex("dbo.Policies", new[] { "User_Id" });
            DropColumn("dbo.Policies", "User_Id");
            CreateIndex("dbo.Claims", "User_Id");
            AddForeignKey("dbo.Claims", "User_Id", "dbo.Users", "Id");
        }
    }
}
