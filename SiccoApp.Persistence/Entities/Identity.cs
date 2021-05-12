using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiccoApp.Persistence
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [Display(Name = "FirstName")]//, ResourceType = typeof(Resources.Resources))]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "LastName")]//, ResourceType = typeof(Resources.Resources))]
        public string LastName { get; set; }

        //[Required]
        //[Display(Name = "Email")]//, ResourceType = typeof(Resources.Resources))]
        //public string Email { get; set; }

        public ICollection<Presentation> Presentations { get; set; }

        //[InverseProperty("TakenFor")]
        //public virtual ICollection<Presentation> PresentationsTakenFor { get; set; }
        //public virtual ICollection<Presentation> PresentationsApprovedFor { get; set; }
    }

    //public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    //{
    //    //public ApplicationDbContext()
    //    //    : base("SiccoAppContextIdentity", throwIfV1Schema: false)
    //    //{

    //    //}
    //    public ApplicationDbContext()
    //        : base("SiccoAppContext", throwIfV1Schema: false)
    //    {
    //    }

    //    public static ApplicationDbContext Create()
    //    {

    //        return new ApplicationDbContext();
    //    }

    //    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    //    {
    //        base.OnModelCreating(modelBuilder);

    //        // your stuff here
    //    }

    //}

    public class IdentityManager
    {
        public bool RoleExists(string name)
        {
            var rm = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(new ApplicationDbContext()));
            return rm.RoleExists(name);
        }


        public bool CreateRole(string name)
        {
            var rm = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(new ApplicationDbContext()));
            var idResult = rm.Create(new IdentityRole(name));
            return idResult.Succeeded;
        }


        public bool CreateUser(ApplicationUser user, string password)
        {
            var um = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var idResult = um.Create(user, password);
            return idResult.Succeeded;
        }

#warning "FEIIIIISIMO"
        public bool UpdateUser(ApplicationUser user)
        {
            var um = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(new ApplicationDbContext()));

            var userExist = um.FindById(user.Id);
            userExist.FirstName = user.FirstName;
            userExist.LastName = user.LastName;
            userExist.Email = user.Email;

            var idResult = um.Update(userExist);
            return idResult.Succeeded;
        }

        public bool DeleteUser(string userId)
        {
            var um = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var idResult = um.Delete(um.FindById(userId));
            return idResult.Succeeded;
        }

        public bool AddUserToRole(string userId, string roleName)
        {
            var um = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var idResult = um.AddToRole(userId, roleName);
            return idResult.Succeeded;
        }

        public bool RemoveUserFromRole(string userId, string roleName)
        {
            var um = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var idResult = um.RemoveFromRole(userId, roleName);
            return idResult.Succeeded;
        }

        //public void ClearUserRoles(string userId)
        //{
        //    var um = new UserManager<ApplicationUser>(
        //        new UserStore<ApplicationUser>(new ApplicationDbContext()));
        //    var user = um.FindById(userId);
        //    var currentRoles = new List<IdentityUserRole>();
        //    currentRoles.AddRange(user.Roles);
        //    foreach (var role in currentRoles)
        //    {
        //        //um.RemoveFromRole(userId, role.Role.Name);
        //        um.RemoveFromRole(userId, role.RoleId);
        //    }
        //}

        public void ClearUserRoles(string userId)
        {
            var um = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(new ApplicationDbContext()));

            um.RemoveFromRoles(userId);

        }
    }
}
