namespace Authority.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductOwnerId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "OwnerId", c => c.Guid(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "OwnerId");
        }
    }
}
