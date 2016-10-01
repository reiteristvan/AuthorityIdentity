namespace AuthorityIdentity.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserInvite : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Authority.Invites",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Email = c.String(nullable: false, maxLength: 256),
                        DomainId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("Authority.Invites");
        }
    }
}
