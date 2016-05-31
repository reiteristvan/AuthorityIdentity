namespace Authority.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductProductStyle : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductStyles",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Logo = c.Binary(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.Id, cascadeDelete: true)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductStyles", "Id", "dbo.Products");
            DropIndex("dbo.ProductStyles", new[] { "Id" });
            DropTable("dbo.ProductStyles");
        }
    }
}
