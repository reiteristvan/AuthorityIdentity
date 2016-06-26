namespace Authority.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PolicyCascadeDeleteWhenDomainDeleted : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Authority.Policies", "DomainId", "Authority.Domains");
            AddForeignKey("Authority.Policies", "DomainId", "Authority.Domains", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("Authority.Policies", "DomainId", "Authority.Domains");
            AddForeignKey("Authority.Policies", "DomainId", "Authority.Domains", "Id");
        }
    }
}
