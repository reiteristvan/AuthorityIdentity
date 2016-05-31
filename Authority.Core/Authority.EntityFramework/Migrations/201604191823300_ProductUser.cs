namespace Authority.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Product_Id", c => c.Guid());
            CreateIndex("dbo.Users", "Product_Id");
            AddForeignKey("dbo.Users", "Product_Id", "dbo.Products", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "Product_Id", "dbo.Products");
            DropIndex("dbo.Users", new[] { "Product_Id" });
            DropColumn("dbo.Users", "Product_Id");
        }
    }
}
