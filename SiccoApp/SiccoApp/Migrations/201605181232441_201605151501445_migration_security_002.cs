namespace SiccoApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _201605151501445_migration_security_002 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "customer_CustomerID", "dbo.Customer");
            RenameColumn(table: "dbo.AspNetUsers", name: "customer_CustomerID", newName: "CustomerID");
            RenameIndex(table: "dbo.AspNetUsers", name: "IX_customer_CustomerID", newName: "IX_CustomerID");
            AddForeignKey("dbo.AspNetUsers", "CustomerID", "dbo.Customer", "CustomerID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "CustomerID", "dbo.Customer");
            RenameIndex(table: "dbo.AspNetUsers", name: "IX_CustomerID", newName: "IX_customer_CustomerID");
            RenameColumn(table: "dbo.AspNetUsers", name: "CustomerID", newName: "customer_CustomerID");
            AddForeignKey("dbo.AspNetUsers", "customer_CustomerID", "dbo.Customer", "CustomerID");
        }
    }
}
