namespace Authority.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PolicyHasProductIdInModel : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Policies", new[] { "Product_Id" });
            RenameColumn(table: "dbo.Policies", name: "Product_Id", newName: "ProductId");
            AlterColumn("dbo.Policies", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Policies", "ProductId", c => c.Guid(nullable: false));
            CreateIndex("dbo.Policies", "ProductId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Policies", new[] { "ProductId" });
            AlterColumn("dbo.Policies", "ProductId", c => c.Guid());
            AlterColumn("dbo.Policies", "Name", c => c.String());
            RenameColumn(table: "dbo.Policies", name: "ProductId", newName: "Product_Id");
            CreateIndex("dbo.Policies", "Product_Id");
        }
    }
}
