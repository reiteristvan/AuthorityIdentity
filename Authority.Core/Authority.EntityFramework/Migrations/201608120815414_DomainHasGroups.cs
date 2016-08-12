namespace Authority.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DomainHasGroups : DbMigration
    {
        public override void Up()
        {
            AddColumn("Authority.Groups", "DomainId", c => c.Guid(nullable: false));
            CreateIndex("Authority.Groups", "DomainId");
            AddForeignKey("Authority.Groups", "DomainId", "Authority.Domains", "Id", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("Authority.Groups", "DomainId", "Authority.Domains");
            DropIndex("Authority.Groups", new[] { "DomainId" });
            DropColumn("Authority.Groups", "DomainId");
        }
    }
}
