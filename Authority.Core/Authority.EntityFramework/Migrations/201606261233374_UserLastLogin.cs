namespace Authority.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserLastLogin : DbMigration
    {
        public override void Up()
        {
            AddColumn("Authority.Users", "LastLogin", c => c.DateTimeOffset(nullable: false, precision: 7));
        }
        
        public override void Down()
        {
            DropColumn("Authority.Users", "LastLogin");
        }
    }
}
