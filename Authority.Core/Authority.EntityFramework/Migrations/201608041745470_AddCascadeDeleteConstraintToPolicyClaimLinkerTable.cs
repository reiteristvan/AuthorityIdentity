namespace Authority.EntityFramework.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddCascadeDeleteConstraintToPolicyClaimLinkerTable : DbMigration
    {
        public override void Up()
        {
            Sql("ALTER TABLE dbo.PolicyAuthorityClaims ADD CONSTRAINT FK_Policy_Claim_CascadeDelete FOREIGN KEY(Policy_Id) REFERENCES Authority.Policies(Id) ON DELETE CASCADE");
        }
        
        public override void Down()
        {
        }
    }
}
