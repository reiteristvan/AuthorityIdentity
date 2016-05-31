namespace Authority.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TablesGetAuthorityPrefix : DbMigration
    {
        public override void Up()
        {
            MoveTable(name: "dbo.Claims", newSchema: "Authority");
            MoveTable(name: "dbo.ClientApplications", newSchema: "Authority");
            MoveTable(name: "dbo.Products", newSchema: "Authority");
            MoveTable(name: "dbo.Policies", newSchema: "Authority");
            MoveTable(name: "dbo.Users", newSchema: "Authority");
            MoveTable(name: "dbo.ProductStyles", newSchema: "Authority");
            MoveTable(name: "dbo.Developers", newSchema: "Authority");
            MoveTable(name: "dbo.Errors", newSchema: "Authority");
        }
        
        public override void Down()
        {
            MoveTable(name: "Authority.Errors", newSchema: "dbo");
            MoveTable(name: "Authority.Developers", newSchema: "dbo");
            MoveTable(name: "Authority.ProductStyles", newSchema: "dbo");
            MoveTable(name: "Authority.Users", newSchema: "dbo");
            MoveTable(name: "Authority.Policies", newSchema: "dbo");
            MoveTable(name: "Authority.Products", newSchema: "dbo");
            MoveTable(name: "Authority.ClientApplications", newSchema: "dbo");
            MoveTable(name: "Authority.Claims", newSchema: "dbo");
        }
    }
}
