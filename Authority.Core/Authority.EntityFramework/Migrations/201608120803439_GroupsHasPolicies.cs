namespace Authority.EntityFramework.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class GroupsHasPolicies : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GroupPolicies",
                c => new
                    {
                        Group_Id = c.Guid(nullable: false),
                        Policy_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Group_Id, t.Policy_Id })
                .ForeignKey("Authority.Groups", t => t.Group_Id, cascadeDelete: true)
                .ForeignKey("Authority.Policies", t => t.Policy_Id, cascadeDelete: true)
                .Index(t => t.Group_Id)
                .Index(t => t.Policy_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GroupPolicies", "Policy_Id", "Authority.Policies");
            DropForeignKey("dbo.GroupPolicies", "Group_Id", "Authority.Groups");
            DropIndex("dbo.GroupPolicies", new[] { "Policy_Id" });
            DropIndex("dbo.GroupPolicies", new[] { "Group_Id" });
            DropTable("dbo.GroupPolicies");
        }
    }
}
