namespace Authority.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddClientIdClientSecretToProduct : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "ClientId", c => c.Guid(nullable: false));
            AddColumn("dbo.Products", "ClientSecret", c => c.Guid(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "ClientSecret");
            DropColumn("dbo.Products", "ClientId");
        }
    }
}
