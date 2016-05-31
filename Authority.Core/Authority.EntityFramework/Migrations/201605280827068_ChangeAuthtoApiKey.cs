namespace Authority.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeAuthtoApiKey : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "ApiKey", c => c.Guid(nullable: false));
            DropColumn("dbo.Products", "ClientId");
            DropColumn("dbo.Products", "ClientSecret");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "ClientSecret", c => c.Guid(nullable: false));
            AddColumn("dbo.Products", "ClientId", c => c.Guid(nullable: false));
            DropColumn("dbo.Products", "ApiKey");
        }
    }
}
