namespace SiccoApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migration_security_004 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "Contractor_ContractorID", "dbo.Contractor");
            RenameColumn(table: "dbo.AspNetUsers", name: "Contractor_ContractorID", newName: "ContractorID");
            RenameIndex(table: "dbo.AspNetUsers", name: "IX_Contractor_ContractorID", newName: "IX_ContractorID");
            AddForeignKey("dbo.AspNetUsers", "ContractorID", "dbo.Contractor", "ContractorID", cascadeDelete: true);
            DropColumn("dbo.AspNetUsers", "CustomerID1");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "CustomerID1", c => c.Int());
            DropForeignKey("dbo.AspNetUsers", "ContractorID", "dbo.Contractor");
            RenameIndex(table: "dbo.AspNetUsers", name: "IX_ContractorID", newName: "IX_Contractor_ContractorID");
            RenameColumn(table: "dbo.AspNetUsers", name: "ContractorID", newName: "Contractor_ContractorID");
            AddForeignKey("dbo.AspNetUsers", "Contractor_ContractorID", "dbo.Contractor", "ContractorID");
        }
    }
}
