using SiccoApp.Models;
using SiccoApp.DAL;
using System.Data.Entity.Migrations;

namespace SiccoApp.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<SiccoAppContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }


        protected override void Seed(SiccoAppContext context)
        {
            //this.AddUserAndRoles();
            this.AddAdminUser();
        }

        bool AddAdminUser()
        {
            bool success = false;
            var idManager = new IdentityManager();

            var newUser = new ApplicationUser()
            {
                UserName = "siccoapp",
                FirstName = "Admin",
                LastName = "Sicco",
                Email = "santiagomauhourat@hotmail.com"
            };

            success = idManager.CreateUser(newUser, "Password1");
            if (!success) return success;

            return success;
        }

        bool AddUserAndRoles()
        {
            bool success = false;

            var idManager = new IdentityManager();
            success = idManager.CreateRole("Admin");
            if (!success == true) return success;

            success = idManager.CreateRole("CanEdit");
            if (!success == true) return success;

            success = idManager.CreateRole("User");
            if (!success) return success;


            var newUser = new ApplicationUser()
            {
                UserName = "sanomar",
                FirstName = "Santiago",
                LastName = "Mauhourat",
                Email = "santiagomauhourat@hotmail.com"
            };

            // Be careful here - you  will need to use a password which will 
            // be valid under the password rules for the application, 
            // or the process will abort:
            success = idManager.CreateUser(newUser, "Password1");
            if (!success) return success;

            success = idManager.AddUserToRole(newUser.Id, "Admin");
            if (!success) return success;

            success = idManager.AddUserToRole(newUser.Id, "CanEdit");
            if (!success) return success;

            success = idManager.AddUserToRole(newUser.Id, "User");
            if (!success) return success;

            return success;
        }
    }
}
