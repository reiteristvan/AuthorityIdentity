namespace AuthorityIdentity.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixPolicyClaimRelationship : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Authority.Claims", "Policy_Id", "Authority.Policies");
            DropIndex("Authority.Claims", new[] { "Policy_Id" });
            CreateTable(
                "dbo.PolicyAuthorityClaims",
                c => new
                    {
                        Policy_Id = c.Guid(nullable: false),
                        AuthorityClaim_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Policy_Id, t.AuthorityClaim_Id })
                .ForeignKey("Authority.Policies", t => t.Policy_Id, cascadeDelete: false)
                .ForeignKey("Authority.Claims", t => t.AuthorityClaim_Id, cascadeDelete: false)
                .Index(t => t.Policy_Id)
                .Index(t => t.AuthorityClaim_Id);
            
            DropColumn("Authority.Claims", "Policy_Id");
        }
        
        public override void Down()
        {
            AddColumn("Authority.Claims", "Policy_Id", c => c.Guid());
            DropForeignKey("dbo.PolicyAuthorityClaims", "AuthorityClaim_Id", "Authority.Claims");
            DropForeignKey("dbo.PolicyAuthorityClaims", "Policy_Id", "Authority.Policies");
            DropIndex("dbo.PolicyAuthorityClaims", new[] { "AuthorityClaim_Id" });
            DropIndex("dbo.PolicyAuthorityClaims", new[] { "Policy_Id" });
            DropTable("dbo.PolicyAuthorityClaims");
            CreateIndex("Authority.Claims", "Policy_Id");
            AddForeignKey("Authority.Claims", "Policy_Id", "Authority.Policies", "Id");
        }
    }
}
