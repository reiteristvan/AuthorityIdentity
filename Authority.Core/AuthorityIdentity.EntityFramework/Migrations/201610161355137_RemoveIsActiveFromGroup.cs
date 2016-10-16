namespace AuthorityIdentity.EntityFramework.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class RemoveIsActiveFromGroup : DbMigration
    {
        public override void Up()
        {
            DropColumn("Authority.Groups", "IsActive");
        }
        
        public override void Down()
        {
            AddColumn("Authority.Groups", "IsActive", c => c.Boolean(nullable: false));
        }
    }
}
