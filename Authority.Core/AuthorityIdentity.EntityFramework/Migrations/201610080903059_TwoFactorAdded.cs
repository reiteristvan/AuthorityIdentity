namespace AuthorityIdentity.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TwoFactorAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("Authority.Users", "IsTwoFactorEnabled", c => c.Boolean(nullable: false));
            AddColumn("Authority.Users", "TwoFactorToken", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Authority.Users", "TwoFactorToken");
            DropColumn("Authority.Users", "IsTwoFactorEnabled");
        }
    }
}
