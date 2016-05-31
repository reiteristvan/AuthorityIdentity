namespace Authority.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductRemoveLandingUrlAddNotificationEmailActivationUrl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "NotificationEmail", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Products", "ActivationUrl", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.Products", "LandingPage");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "LandingPage", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.Products", "ActivationUrl");
            DropColumn("dbo.Products", "NotificationEmail");
        }
    }
}
