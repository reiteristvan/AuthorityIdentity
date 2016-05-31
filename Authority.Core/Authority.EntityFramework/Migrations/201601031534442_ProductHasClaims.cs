namespace Authority.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductHasClaims : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Claims", "Product_Id", c => c.Guid());
            CreateIndex("dbo.Claims", "Product_Id");
            AddForeignKey("dbo.Claims", "Product_Id", "dbo.Products", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Claims", "Product_Id", "dbo.Products");
            DropIndex("dbo.Claims", new[] { "Product_Id" });
            DropColumn("dbo.Claims", "Product_Id");
        }
    }
}
