namespace AuthorityIdentity.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedExpirationToInvitation : DbMigration
    {
        public override void Up()
        {
            AddColumn("Authority.Invites", "Expire", c => c.DateTimeOffset(precision: 7));
        }
        
        public override void Down()
        {
            DropColumn("Authority.Invites", "Expire");
        }
    }
}
