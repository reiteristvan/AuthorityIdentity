namespace Authority.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductSiteUrlAndLandingPage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "SiteUrl", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Products", "LandingPage", c => c.String(nullable: false, maxLength: 128));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "LandingPage");
            DropColumn("dbo.Products", "SiteUrl");
        }
    }
}
