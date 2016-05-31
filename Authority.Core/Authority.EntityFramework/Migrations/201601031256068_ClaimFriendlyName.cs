namespace Authority.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ClaimFriendlyName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Claims", "FriendlyName", c => c.String(nullable: false, maxLength: 128));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Claims", "FriendlyName");
        }
    }
}
