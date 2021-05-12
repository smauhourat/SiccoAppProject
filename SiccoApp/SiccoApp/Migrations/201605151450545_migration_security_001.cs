namespace SiccoApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migration_security_001 : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.BusinessType",
            //    c => new
            //        {
            //            BusinessTypeID = c.Int(nullable: false, identity: true),
            //            BusinessTypeCode = c.String(),
            //            Description = c.String(),
            //        })
            //    .PrimaryKey(t => t.BusinessTypeID);
            
            //CreateTable(
            //    "dbo.Contractor",
            //    c => new
            //        {
            //            ContractorID = c.Int(nullable: false, identity: true),
            //            CompanyName = c.String(nullable: false, maxLength: 150),
            //            TaxIdNumber = c.String(nullable: false, maxLength: 20),
            //            CountryID = c.Int(),
            //            StateID = c.Int(),
            //            City = c.String(),
            //            Address = c.String(),
            //            PhoneNumber = c.String(maxLength: 20),
            //            EmergencyPhoneNumber = c.String(maxLength: 20),
            //            Email = c.String(),
            //            Active = c.Byte(),
            //            CreationDate = c.DateTime(),
            //            CreationUser = c.String(),
            //            ModifiedDate = c.DateTime(),
            //            ModifiedUser = c.String(),
            //            BusinessTypeID = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => t.ContractorID)
            //    .ForeignKey("dbo.BusinessType", t => t.BusinessTypeID, cascadeDelete: true)
            //    .ForeignKey("dbo.Country", t => t.CountryID)
            //    .ForeignKey("dbo.State", t => t.StateID)
            //    .Index(t => t.CountryID)
            //    .Index(t => t.StateID)
            //    .Index(t => t.BusinessTypeID);
            
            //CreateTable(
            //    "dbo.Contract",
            //    c => new
            //        {
            //            ContractID = c.Int(nullable: false, identity: true),
            //            ContractorID = c.Int(nullable: false),
            //            CustomerID = c.Int(nullable: false),
            //            StartDate = c.DateTime(),
            //            EndDate = c.Int(),
            //            Score = c.Int(),
            //        })
            //    .PrimaryKey(t => t.ContractID)
            //    .ForeignKey("dbo.Contractor", t => t.ContractorID, cascadeDelete: true)
            //    .ForeignKey("dbo.Customer", t => t.CustomerID, cascadeDelete: true)
            //    .Index(t => t.ContractorID)
            //    .Index(t => t.CustomerID);
            
            //CreateTable(
            //    "dbo.Customer",
            //    c => new
            //        {
            //            CustomerID = c.Int(nullable: false, identity: true),
            //            CompanyName = c.String(nullable: false, maxLength: 150),
            //            TaxIdNumber = c.String(nullable: false, maxLength: 20),
            //            CountryID = c.Int(),
            //            StateID = c.Int(),
            //            City = c.String(),
            //            Address = c.String(),
            //            PhoneNumber = c.String(maxLength: 20),
            //            Active = c.Byte(),
            //            CreationDate = c.DateTime(),
            //            CreationUser = c.String(),
            //            ModifiedDate = c.DateTime(),
            //            ModifiedUser = c.String(),
            //        })
            //    .PrimaryKey(t => t.CustomerID)
            //    .ForeignKey("dbo.State", t => t.StateID)
            //    .ForeignKey("dbo.Country", t => t.CountryID)
            //    .Index(t => t.CountryID)
            //    .Index(t => t.StateID);
            
            //CreateTable(
            //    "dbo.Country",
            //    c => new
            //        {
            //            CountryID = c.Int(nullable: false, identity: true),
            //            CountryName = c.String(),
            //        })
            //    .PrimaryKey(t => t.CountryID);
            
            //CreateTable(
            //    "dbo.State",
            //    c => new
            //        {
            //            StateID = c.Int(nullable: false, identity: true),
            //            StateName = c.String(),
            //            CountryID = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => t.StateID)
            //    .ForeignKey("dbo.Country", t => t.CountryID, cascadeDelete: true)
            //    .Index(t => t.CountryID);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        Email = c.String(nullable: false, maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        customer_CustomerID = c.Int(),
                        contractor_ContractorID = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customer", t => t.customer_CustomerID)
                .ForeignKey("dbo.Contractor", t => t.contractor_ContractorID)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex")
                .Index(t => t.customer_CustomerID)
                .Index(t => t.contractor_ContractorID);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            //CreateTable(
            //    "dbo.Presentation",
            //    c => new
            //        {
            //            PresentationID = c.Int(nullable: false, identity: true),
            //            ContractID = c.Int(nullable: false),
            //            EmployeeContractID = c.Int(),
            //            VehicleContractID = c.Int(),
            //            DocumentationBusinessTypeID = c.Int(nullable: false),
            //            PresentationStateID = c.Int(nullable: false),
            //            PresentationDate = c.DateTime(nullable: false),
            //            DocumentFiles = c.String(),
            //            ApprovedFor = c.Int(),
            //            ApprovedDate = c.DateTime(),
            //            RejectedFor = c.Int(),
            //            RejectedDate = c.DateTime(),
            //            Observations = c.String(),
            //        })
            //    .PrimaryKey(t => t.PresentationID)
            //    .ForeignKey("dbo.Contract", t => t.ContractID, cascadeDelete: true)
            //    .ForeignKey("dbo.DocumentationBusinessType", t => t.DocumentationBusinessTypeID, cascadeDelete: true)
            //    .ForeignKey("dbo.PresentationState", t => t.PresentationStateID, cascadeDelete: true)
            //    .Index(t => t.ContractID)
            //    .Index(t => t.DocumentationBusinessTypeID)
            //    .Index(t => t.PresentationStateID);
            
            //CreateTable(
            //    "dbo.DocumentationBusinessType",
            //    c => new
            //        {
            //            DocumentationBusinessTypeID = c.Int(nullable: false, identity: true),
            //            DocumentationID = c.Int(nullable: false),
            //            BusinessTypeID = c.Int(nullable: false),
            //            Importance = c.Byte(nullable: false),
            //        })
            //    .PrimaryKey(t => t.DocumentationBusinessTypeID)
            //    .ForeignKey("dbo.BusinessType", t => t.BusinessTypeID, cascadeDelete: true)
            //    .ForeignKey("dbo.Documentation", t => t.DocumentationID, cascadeDelete: true)
            //    .Index(t => t.DocumentationID)
            //    .Index(t => t.BusinessTypeID);
            
            //CreateTable(
            //    "dbo.Documentation",
            //    c => new
            //        {
            //            DocumentationID = c.Int(nullable: false, identity: true),
            //            DocumentationCode = c.String(nullable: false, maxLength: 20),
            //            Description = c.String(nullable: false, maxLength: 150),
            //            EntityTypeID = c.Int(nullable: false),
            //            DocumentationPeriodicityID = c.Int(),
            //        })
            //    .PrimaryKey(t => t.DocumentationID)
            //    .ForeignKey("dbo.DocumentationPeriodicity", t => t.DocumentationPeriodicityID)
            //    .ForeignKey("dbo.EntityType", t => t.EntityTypeID, cascadeDelete: true)
            //    .Index(t => t.EntityTypeID)
            //    .Index(t => t.DocumentationPeriodicityID);
            
            //CreateTable(
            //    "dbo.DocumentationPeriodicity",
            //    c => new
            //        {
            //            DocumentationPeriodicityID = c.Int(nullable: false, identity: true),
            //            Description = c.String(),
            //        })
            //    .PrimaryKey(t => t.DocumentationPeriodicityID);
            
            //CreateTable(
            //    "dbo.DocumentationTemplate",
            //    c => new
            //        {
            //            DocumentationTemplateID = c.Int(nullable: false, identity: true),
            //            DocumentationTemplateCode = c.String(nullable: false, maxLength: 20),
            //            Description = c.String(nullable: false, maxLength: 150),
            //            EntityTypeID = c.Int(nullable: false),
            //            DocumentationPeriodicityID = c.Int(),
            //        })
            //    .PrimaryKey(t => t.DocumentationTemplateID)
            //    .ForeignKey("dbo.DocumentationPeriodicity", t => t.DocumentationPeriodicityID)
            //    .ForeignKey("dbo.EntityType", t => t.EntityTypeID, cascadeDelete: true)
            //    .Index(t => t.EntityTypeID)
            //    .Index(t => t.DocumentationPeriodicityID);
            
            //CreateTable(
            //    "dbo.DocumentationBusinessTypeTemplate",
            //    c => new
            //        {
            //            DocumentationBusinessTypeTemplateID = c.Int(nullable: false, identity: true),
            //            DocumentationTemplateID = c.Int(nullable: false),
            //            BusinessTypeTemplateID = c.Int(nullable: false),
            //            Importance = c.Byte(nullable: false),
            //        })
            //    .PrimaryKey(t => t.DocumentationBusinessTypeTemplateID)
            //    .ForeignKey("dbo.BusinessTypeTemplate", t => t.BusinessTypeTemplateID, cascadeDelete: true)
            //    .ForeignKey("dbo.DocumentationTemplate", t => t.DocumentationTemplateID, cascadeDelete: true)
            //    .Index(t => t.DocumentationTemplateID)
            //    .Index(t => t.BusinessTypeTemplateID);
            
            //CreateTable(
            //    "dbo.BusinessTypeTemplate",
            //    c => new
            //        {
            //            BusinessTypeTemplateID = c.Int(nullable: false, identity: true),
            //            BusinessTypeTemplateCode = c.String(nullable: false, maxLength: 20),
            //            Description = c.String(nullable: false, maxLength: 150),
            //        })
            //    .PrimaryKey(t => t.BusinessTypeTemplateID);
            
            //CreateTable(
            //    "dbo.EntityType",
            //    c => new
            //        {
            //            EntityTypeID = c.Int(nullable: false, identity: true),
            //            Description = c.String(),
            //        })
            //    .PrimaryKey(t => t.EntityTypeID);
            
            //CreateTable(
            //    "dbo.PresentationState",
            //    c => new
            //        {
            //            PresentationStateID = c.Int(nullable: false, identity: true),
            //            Description = c.String(),
            //        })
            //    .PrimaryKey(t => t.PresentationStateID);
            
            //CreateTable(
            //    "dbo.Employee",
            //    c => new
            //        {
            //            EmployeeID = c.Int(nullable: false, identity: true),
            //            ContractorID = c.Int(nullable: false),
            //            IdentificationNumberTypeID = c.Int(nullable: false),
            //            IdentificationNumber = c.String(),
            //            FirstName = c.String(),
            //            LastName = c.String(),
            //            MaritalStatus = c.String(),
            //            Gender = c.String(),
            //            BirthDate = c.DateTime(),
            //            CountryID = c.Int(),
            //            StateID = c.Int(),
            //            City = c.String(),
            //            Address = c.String(),
            //            PhoneNumber = c.String(),
            //            Active = c.Byte(),
            //            CreationDate = c.DateTime(),
            //            CreationUser = c.String(),
            //            ModifiedDate = c.DateTime(),
            //            ModifiedUser = c.String(),
            //        })
            //    .PrimaryKey(t => t.EmployeeID)
            //    .ForeignKey("dbo.Contractor", t => t.ContractorID, cascadeDelete: true)
            //    .Index(t => t.ContractorID);
            
            //CreateTable(
            //    "dbo.Vehicle",
            //    c => new
            //        {
            //            VehicleID = c.Int(nullable: false, identity: true),
            //            ContractorID = c.Int(nullable: false),
            //            IdentificationNumber = c.String(),
            //            Active = c.Byte(),
            //            CreationDate = c.DateTime(),
            //            CreationUser = c.String(),
            //            ModifiedDate = c.DateTime(),
            //            ModifiedUser = c.String(),
            //        })
            //    .PrimaryKey(t => t.VehicleID)
            //    .ForeignKey("dbo.Contractor", t => t.ContractorID, cascadeDelete: true)
            //    .Index(t => t.ContractorID);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            //DropForeignKey("dbo.Vehicle", "ContractorID", "dbo.Contractor");
            DropForeignKey("dbo.AspNetUsers", "contractor_ContractorID", "dbo.Contractor");
            //DropForeignKey("dbo.Contractor", "StateID", "dbo.State");
            //DropForeignKey("dbo.Employee", "ContractorID", "dbo.Contractor");
            //DropForeignKey("dbo.Contractor", "CountryID", "dbo.Country");
            //DropForeignKey("dbo.Presentation", "PresentationStateID", "dbo.PresentationState");
            //DropForeignKey("dbo.Presentation", "DocumentationBusinessTypeID", "dbo.DocumentationBusinessType");
            //DropForeignKey("dbo.Documentation", "EntityTypeID", "dbo.EntityType");
            //DropForeignKey("dbo.DocumentationTemplate", "EntityTypeID", "dbo.EntityType");
            //DropForeignKey("dbo.DocumentationTemplate", "DocumentationPeriodicityID", "dbo.DocumentationPeriodicity");
            //DropForeignKey("dbo.DocumentationBusinessTypeTemplate", "DocumentationTemplateID", "dbo.DocumentationTemplate");
            //DropForeignKey("dbo.DocumentationBusinessTypeTemplate", "BusinessTypeTemplateID", "dbo.BusinessTypeTemplate");
            //DropForeignKey("dbo.Documentation", "DocumentationPeriodicityID", "dbo.DocumentationPeriodicity");
            //DropForeignKey("dbo.DocumentationBusinessType", "DocumentationID", "dbo.Documentation");
            //DropForeignKey("dbo.DocumentationBusinessType", "BusinessTypeID", "dbo.BusinessType");
            //DropForeignKey("dbo.Presentation", "ContractID", "dbo.Contract");
            DropForeignKey("dbo.AspNetUsers", "customer_CustomerID", "dbo.Customer");
            //DropForeignKey("dbo.Customer", "CountryID", "dbo.Country");
            //DropForeignKey("dbo.State", "CountryID", "dbo.Country");
            //DropForeignKey("dbo.Customer", "StateID", "dbo.State");
            //DropForeignKey("dbo.Contract", "CustomerID", "dbo.Customer");
            //DropForeignKey("dbo.Contract", "ContractorID", "dbo.Contractor");
            //DropForeignKey("dbo.Contractor", "BusinessTypeID", "dbo.BusinessType");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            //DropIndex("dbo.Vehicle", new[] { "ContractorID" });
            //DropIndex("dbo.Employee", new[] { "ContractorID" });
            //DropIndex("dbo.DocumentationBusinessTypeTemplate", new[] { "BusinessTypeTemplateID" });
            //DropIndex("dbo.DocumentationBusinessTypeTemplate", new[] { "DocumentationTemplateID" });
            //DropIndex("dbo.DocumentationTemplate", new[] { "DocumentationPeriodicityID" });
            //DropIndex("dbo.DocumentationTemplate", new[] { "EntityTypeID" });
            //DropIndex("dbo.Documentation", new[] { "DocumentationPeriodicityID" });
            //DropIndex("dbo.Documentation", new[] { "EntityTypeID" });
            //DropIndex("dbo.DocumentationBusinessType", new[] { "BusinessTypeID" });
            //DropIndex("dbo.DocumentationBusinessType", new[] { "DocumentationID" });
            //DropIndex("dbo.Presentation", new[] { "PresentationStateID" });
            //DropIndex("dbo.Presentation", new[] { "DocumentationBusinessTypeID" });
            //DropIndex("dbo.Presentation", new[] { "ContractID" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", new[] { "contractor_ContractorID" });
            DropIndex("dbo.AspNetUsers", new[] { "customer_CustomerID" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            //DropIndex("dbo.State", new[] { "CountryID" });
            //DropIndex("dbo.Customer", new[] { "StateID" });
            //DropIndex("dbo.Customer", new[] { "CountryID" });
            //DropIndex("dbo.Contract", new[] { "CustomerID" });
            //DropIndex("dbo.Contract", new[] { "ContractorID" });
            //DropIndex("dbo.Contractor", new[] { "BusinessTypeID" });
            //DropIndex("dbo.Contractor", new[] { "StateID" });
            //DropIndex("dbo.Contractor", new[] { "CountryID" });
            DropTable("dbo.AspNetRoles");
            //DropTable("dbo.Vehicle");
            //DropTable("dbo.Employee");
            //DropTable("dbo.PresentationState");
            //DropTable("dbo.EntityType");
            //DropTable("dbo.BusinessTypeTemplate");
            //DropTable("dbo.DocumentationBusinessTypeTemplate");
            //DropTable("dbo.DocumentationTemplate");
            //DropTable("dbo.DocumentationPeriodicity");
            //DropTable("dbo.Documentation");
            //DropTable("dbo.DocumentationBusinessType");
            //DropTable("dbo.Presentation");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            //DropTable("dbo.State");
            //DropTable("dbo.Country");
            //DropTable("dbo.Customer");
            //DropTable("dbo.Contract");
            //DropTable("dbo.Contractor");
            //DropTable("dbo.BusinessType");
        }
    }
}
