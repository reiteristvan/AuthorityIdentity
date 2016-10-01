namespace AuthorityIdentity.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveUserCountFromGroup : DbMigration
    {
        public override void Up()
        {
            DropColumn("Authority.Groups", "UserCount");
        }
        
        public override void Down()
        {
            AddColumn("Authority.Groups", "UserCount", c => c.Int(nullable: false));
        }
    }
}
