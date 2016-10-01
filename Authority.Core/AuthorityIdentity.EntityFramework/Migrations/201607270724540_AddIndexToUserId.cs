namespace AuthorityIdentity.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIndexToUserId : DbMigration
    {
        public override void Up()
        {
            CreateIndex("Authority.Users", "Id", name: "IX_Authority.Users_Id");
        }
        
        public override void Down()
        {
            DropIndex("Authority.Users", "IX_Authority.Users_Id");
        }
    }
}
