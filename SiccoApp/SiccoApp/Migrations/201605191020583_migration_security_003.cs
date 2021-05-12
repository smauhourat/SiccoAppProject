namespace SiccoApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migration_security_003 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.AspNetUsers", new[] { "contractor_ContractorID" });
            AddColumn("dbo.AspNetUsers", "CustomerID1", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "Contractor_ContractorID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.AspNetUsers", new[] { "Contractor_ContractorID" });
            DropColumn("dbo.AspNetUsers", "CustomerID1");
            CreateIndex("dbo.AspNetUsers", "contractor_ContractorID");
        }
    }
}
