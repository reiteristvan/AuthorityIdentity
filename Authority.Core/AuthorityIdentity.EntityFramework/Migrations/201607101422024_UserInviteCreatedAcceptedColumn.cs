namespace AuthorityIdentity.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserInviteCreatedAcceptedColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("Authority.Invites", "Created", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("Authority.Invites", "Accepted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Authority.Invites", "Accepted");
            DropColumn("Authority.Invites", "Created");
        }
    }
}
