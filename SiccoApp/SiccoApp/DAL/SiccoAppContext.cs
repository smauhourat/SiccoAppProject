using SiccoApp.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;

namespace SiccoApp.DAL
{
    public class SiccoAppContext : ApplicationDbContext 
    {
        public DbSet<BusinessTypeTemplate> BusinessTypeTemplate { get; set; }


        public DbSet<BusinessType> BusinessTypes { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<DocumentationTemplate> DocumentationTemplates { get; set; }
        //public virtual DbSet<Contract> Contracts { get; set; }
        public DbSet<Contractor> Contractors { get; set; }
        //public virtual DbSet<Customer> Customers { get; set; }
        public DbSet<Documentation> Documentations { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        //public virtual DbSet<Presentation> Presentations { get; set; }
        //public virtual DbSet<PresentationState> PresentationStates { get; set; }
        //public virtual DbSet<Vehicle> Vehicles { get; set; }

        //public DbSet<Country> Countries { get; set; }
        //public DbSet<State> States { get; set; }

        public DbSet<DocumentationPeriodicity> DocumentationPeriodicities { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<EntityType> EntityTypes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            //modelBuilder.Entity<IdentityRole>().HasKey<string>(r => r.Id);
            //modelBuilder.Entity<IdentityUserRole>().HasKey(r => new { r.RoleId, r.UserId });
            
            base.OnModelCreating(modelBuilder);
        }

        public System.Data.Entity.DbSet<SiccoApp.Models.CustomerUser> ApplicationUsers { get; set; }
    }
}
