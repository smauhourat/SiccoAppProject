using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Runtime.Remoting.Messaging;
using System.Data.Entity.Infrastructure;

namespace SiccoApp.Persistence
{
    public interface ISiccoAppContext
    {
        DbSet<BusinessTypeTemplate> BusinessTypeTemplates { get; set; }
        DbSet<BusinessType> BusinessTypes { get; set; }
        DbSet<EmployeeRelationshipType> EmployeeRelationshipTypes { get; set; }
        DbSet<DocumentationTemplate> DocumentationTemplates { get; set; }
        DbSet<Documentation> Documentations { get; set; }
        DbSet<EntityType> EntityTypes { get; set; }
        DbSet<DocumentationPeriodicity> DocumentationPeriodicitys { get; set; }
        DbSet<DocumentationBusinessTypeTemplate> DocumentationBusinessTypeTemplates { get; set; }
        DbSet<Customer> Customers { get; set; }
        DbSet<Contractor> Contractors { get; set; }
        DbSet<Contract> Contracts { get; set; }
        DbSet<Country> Countries { get; set; }
        DbSet<State> States { get; set; }
        DbSet<CustomerUser> ApplicationUsers { get; set; }
        DbSet<Employee> Employees { get; set; }
        DbSet<Vehicle> Vehicles { get; set; }
        DbSet<EmployeeContract> EmployeesContracts { get; set; }
        DbSet<VehicleContract> VehiclesContracts { get; set; }
        DbSet<Requirement> Requirements { get; set; }
        DbSet<Presentation> Presentations { get; set; }
        DbSet<PresentationAction> PresentationActions { get; set; }
        DbSet<DocumentationBusinessType> DocumentationBusinessTypes { get; set; }
        DbSet<CustomerAuditor> CustomerAuditors { get; set; }
        DbSet<Period> Periods { get; set; }
        DbSet<DocumentationImportance> DocumentationImportances { get; set; }
    }

    public class SiccoAppContext : ApplicationDbContext//, ISiccoAppContext
    {
        //public SiccoAppContext()
        //    : base("name=SiccoAppContext")
        //{
        //}
        public DbSet<LogRecord> LogRecords { get; set; }
        public DbSet<EmailAccount> EmailAccounts { get; set; }
        public DbSet<BusinessTypeTemplate> BusinessTypeTemplates { get; set; }
        public DbSet<BusinessType> BusinessTypes { get; set; }
        public DbSet<EmployeeRelationshipType> EmployeeRelationshipTypes { get; set; }
        public DbSet<DocumentationTemplate> DocumentationTemplates { get; set; }
        public DbSet<Documentation> Documentations { get; set; }
        public DbSet<DocumentationResume> DocumentationsResume { get; set; }
        public DbSet<EntityType> EntityTypes { get; set; }
        public DbSet<DocumentationPeriodicity> DocumentationPeriodicitys { get; set; }
        public DbSet<DocumentationBusinessTypeTemplate> DocumentationBusinessTypeTemplates { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Contractor> Contractors { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<CustomerUser> ApplicationUsers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<EmployeeContract> EmployeesContracts { get; set; }
        public DbSet<VehicleContract> VehiclesContracts { get; set; }
        public DbSet<Requirement> Requirements { get; set; }
        public DbSet<Presentation> Presentations { get; set; }
        public DbSet<PresentationAction> PresentationActions { get; set; }
        public DbSet<DocumentationBusinessType> DocumentationBusinessTypes { get; set; }
        public DbSet<CustomerAuditor> CustomerAuditors { get; set; }
        public DbSet<Period> Periods { get; set; }
        public DbSet<DocumentationImportance> DocumentationImportances { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            //modelBuilder.Entity<IdentityRole>().HasKey<string>(r => r.Id);
            //modelBuilder.Entity<IdentityUserRole>().HasKey(r => new { r.RoleId, r.UserId });

            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            //modelBuilder.Entity<Presentation>().HasRequired(m => m.TakenFor).WithMany(m => m.Presentations).HasForeignKey(m => m.TakenForID);
            //modelBuilder.Entity<Presentation>().HasOptional(m => m.ApprovedFor).WithMany(m => m.Presentations).HasForeignKey(m => m.ApprovedForID);

            //http://stackoverflow.com/questions/28570916/defining-multiple-foreign-key-for-the-same-table-in-entity-framework-code-first
            modelBuilder.Entity<Presentation>().HasRequired(m => m.TakenFor).WithMany(m => m.Presentations).HasForeignKey(m => m.TakenForID);
            modelBuilder.Entity<Presentation>().HasRequired(m => m.ApprovedFor).WithMany().HasForeignKey(m => m.ApprovedForID);

            //modelBuilder.Entity<Presentation>()
            //            .HasRequired(m => m.TakenFor)
            //            .WithMany(t => t.PresentationsTakenFor)
            //            .HasForeignKey(m => m.TakenForID)
            //            .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Presentation>()
            //            .HasRequired(m => m.ApprovedFor)
            //            .WithMany(t => t.PresentationsApprovedFor)
            //            .HasForeignKey(m => m.ApprovedForID)
            //            .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Presentation>()
            //            .HasOptional(x => x.TakenFor)
            //            .WithMany()
            //            .HasForeignKey(u => u.TakenForID);
            //modelBuilder.Entity<Presentation>()
            //            .HasOptional(x => x.ApprovedFor)
            //            .WithMany()
            //            .HasForeignKey(u => u.ApprovedForID);

            base.OnModelCreating(modelBuilder);
        }

    }

    // EF follows a Code based Configration model and will look for a class that
    // derives from DbConfiguration for executing any Connection Resiliency strategies
    public class SiccoAppConfiguration : DbConfiguration
    {
        public SiccoAppConfiguration()
        {
            //posta
            //SetExecutionStrategy("System.Data.SqlClient", () => new SqlAzureExecutionStrategy());

            this.SetExecutionStrategy("System.Data.SqlClient", () => SuspendExecutionStrategy
              ? (IDbExecutionStrategy)new DefaultExecutionStrategy()
              : new SqlAzureExecutionStrategy());

            ////https://msdn.microsoft.com/en-us/data/dn456835
            ////https://msdn.microsoft.com/en-us/data/jj680699

            ////maximum number of retries to 1 and the maximum delay to 30 seconds
            //SetExecutionStrategy(
            //    "System.Data.SqlClient",
            //    () => new SqlAzureExecutionStrategy(1, TimeSpan.FromSeconds(30)));
        }

        public static bool SuspendExecutionStrategy
        {
            get
            {
                return (bool?)CallContext.LogicalGetData("SuspendExecutionStrategy") ?? false;
            }
            set
            {
                CallContext.LogicalSetData("SuspendExecutionStrategy", value);
            }
        }
    }
}
