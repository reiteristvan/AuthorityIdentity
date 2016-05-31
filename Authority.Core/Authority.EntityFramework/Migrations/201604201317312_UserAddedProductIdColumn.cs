namespace Authority.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserAddedProductIdColumn : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Users", new[] { "Product_Id" });
            RenameColumn(table: "dbo.Users", name: "Product_Id", newName: "ProductId");
            AlterColumn("dbo.Users", "ProductId", c => c.Guid(nullable: false));
            CreateIndex("dbo.Users", "ProductId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Users", new[] { "ProductId" });
            AlterColumn("dbo.Users", "ProductId", c => c.Guid());
            RenameColumn(table: "dbo.Users", name: "ProductId", newName: "Product_Id");
            CreateIndex("dbo.Users", "Product_Id");
        }
    }
}
