namespace Authority.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveErrorEntity : DbMigration
    {
        public override void Up()
        {
            DropTable("Authority.Errors");
        }
        
        public override void Down()
        {
            CreateTable(
                "Authority.Errors",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Type = c.String(nullable: false, maxLength: 256),
                        StackTrace = c.String(nullable: false),
                        Message = c.String(nullable: false),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
    }
}
