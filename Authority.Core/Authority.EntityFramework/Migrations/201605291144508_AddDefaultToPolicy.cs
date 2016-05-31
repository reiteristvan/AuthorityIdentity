namespace Authority.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDefaultToPolicy : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Policies", "Default", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Policies", "Default");
        }
    }
}
