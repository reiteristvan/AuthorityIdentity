namespace Authority.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ErrorAddDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Errors", "Date", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Errors", "Date");
        }
    }
}
