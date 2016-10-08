namespace AuthorityIdentity.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TwoFactorAddedStep2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Authority.Users", "TwoFactorType", c => c.Int(nullable: false));
            AddColumn("Authority.Users", "TwoFactorTarget", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Authority.Users", "TwoFactorTarget");
            DropColumn("Authority.Users", "TwoFactorType");
        }
    }
}
