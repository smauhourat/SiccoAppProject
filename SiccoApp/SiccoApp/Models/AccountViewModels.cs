using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using SiccoApp.Persistence;

namespace SiccoApp.Models
{
    public class ManageUserViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage =
            "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage =
            "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }


    public class LoginViewModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "UserName", ResourceType = typeof(Resources.Resources))]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage =
            "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(Resources.Resources))]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword", ResourceType = typeof(Resources.Resources))]
        [Compare("Password", ErrorMessageResourceName = "ConfirmPasswordNoMatch", ErrorMessageResourceType = typeof(Resources.Resources))]
        public string ConfirmPassword { get; set; }

        // New Fields added to extend Application User class:

        [Required]
        [Display(Name = "FirstName", ResourceType = typeof(Resources.Resources))]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "LastName", ResourceType = typeof(Resources.Resources))]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Email", ResourceType = typeof(Resources.Resources))]
        public string Email { get; set; }

        // Return a pre-poulated instance of ApplicationUser:
        public virtual ApplicationUser GetUser()
        {
            var user = new AdminUser()
            {
                UserName = this.UserName,
                FirstName = this.FirstName,
                LastName = this.LastName,
                Email = this.Email,
            };
            return user;
        }
    }

    public class RegisterCustomerUserViewModel : RegisterViewModel
    {
        public int CustomerID { get; set; }

        public override ApplicationUser GetUser()
        {
            var user = new CustomerUser()
            {
                UserName = this.UserName,
                FirstName = this.FirstName,
                LastName = this.LastName,
                Email = this.Email
            };
            return user;
        }
    }

    public class RegisterContractorUserViewModel : RegisterViewModel
    {
        public int ContractorID { get; set; }

        public override ApplicationUser GetUser()
        {
            var user = new ContractorUser()
            {
                UserName = this.UserName,
                FirstName = this.FirstName,
                LastName = this.LastName,
                Email = this.Email
            };
            return user;
        }
    }

    public class RegisterCustomerAuditorUserViewModel : RegisterViewModel
    {
        public int CustomerID { get; set; }

        public override ApplicationUser GetUser()
        {
            var user = new AdminUser()
            {
                UserName = this.UserName,
                FirstName = this.FirstName,
                LastName = this.LastName,
                Email = this.Email
            };
            return user;
        }
    }

    //public class EditCustomerAuditorUserViewModel
    //{
    //    public EditCustomerAuditorUserViewModel() { }

    //    // Allow Initialization with an instance of ApplicationUser:
    //    public EditCustomerAuditorUserViewModel(ContractorUser user)
    //    {
    //        this.ContractorID = user.ContractorID;
    //        this.UserName = user.UserName;
    //        this.FirstName = user.FirstName;
    //        this.LastName = user.LastName;
    //        this.Email = user.Email;
    //    }

    //    [Required]
    //    [Display(Name = "ContractorID")]
    //    public int ContractorID { get; set; }

    //    [Required]
    //    [Display(Name = "User Name")]
    //    public string UserName { get; set; }

    //    [Required]
    //    [Display(Name = "First Name")]
    //    public string FirstName { get; set; }

    //    [Required]
    //    [Display(Name = "Last Name")]
    //    public string LastName { get; set; }

    //    [Required]
    //    public string Email { get; set; }
    //}

    public class RegisterCustomerAdminUserViewModel : RegisterViewModel
    {
        public int CustomerID { get; set; }

        public override ApplicationUser GetUser()
        {
            var user = new AdminUser()
            {
                UserName = this.UserName,
                FirstName = this.FirstName,
                LastName = this.LastName,
                Email = this.Email
            };
            return user;
        }
    }

    public class EditUserViewModel
    {
        public EditUserViewModel() { }

        // Allow Initialization with an instance of ApplicationUser:
        public EditUserViewModel(ApplicationUser user)
        {
            this.Id = user.Id;
            this.UserName = user.UserName;
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;
            this.Email = user.Email;
        }

        public string Id { get; set; }

        [Required]
        [Display(Name = "UserName", ResourceType = typeof(Resources.Resources))]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "FirstName", ResourceType = typeof(Resources.Resources))]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "LastName", ResourceType = typeof(Resources.Resources))]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Email", ResourceType = typeof(Resources.Resources))]
        public string Email { get; set; }
    }


    public class SelectUserRolesViewModel
    {
        public SelectUserRolesViewModel()
        {
            this.Roles = new List<SelectRoleEditorViewModel>();
        }


        //// Enable initialization with an instance of ApplicationUser:
        //public SelectUserRolesViewModel(ApplicationUser user)
        //    : this()
        //{
        //    this.UserName = user.UserName;
        //    this.FirstName = user.FirstName;
        //    this.LastName = user.LastName;

        //    var Db = new ApplicationDbContext();

        //    // Add all available roles to the list of EditorViewModels:
        //    var allRoles = Db.Roles;
        //    foreach (var role in allRoles)
        //    {
        //        // An EditorViewModel will be used by Editor Template:
        //        var rvm = new SelectRoleEditorViewModel(role);
        //        this.Roles.Add(rvm);
        //    }

        //    // Set the Selected property to true for those roles for 
        //    // which the current user is a member:
        //    foreach (var userRole in user.Roles)
        //    {
        //        //OJOOOO
        //        var checkUserRole =
        //            this.Roles.Find(r => r.RoleName == userRole.Role.Name);
        //        checkUserRole.Selected = true;
        //    }
        //}

        // Enable initialization with an instance of ApplicationUser:
        public SelectUserRolesViewModel(ApplicationUser user)
            : this()
        {
            this.Id = user.Id;
            this.UserName = user.UserName;
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;

            var Db = new ApplicationDbContext();

            // Add all available roles to the list of EditorViewModels:
            var allRoles = Db.Roles;
            foreach (var role in allRoles)
            {
                // An EditorViewModel will be used by Editor Template:
                var rvm = new SelectRoleEditorViewModel(role);
                this.Roles.Add(rvm);
            }

            // Set the Selected property to true for those roles for 
            // which the current user is a member:
            foreach (var userRole in user.Roles)
            {
                //OJOOOO
                var checkUserRole =
                    this.Roles.Find(r => r.RoleId == userRole.RoleId);
                checkUserRole.Selected = true;
            }
        }

        public string Id { get; set; }
        [Display(Name = "UserName", ResourceType = typeof(Resources.Resources))]
        public string UserName { get; set; }

        [Display(Name = "FirstName", ResourceType = typeof(Resources.Resources))]
        public string FirstName { get; set; }

        [Display(Name = "LastName", ResourceType = typeof(Resources.Resources))]
        public string LastName { get; set; }

        public List<SelectRoleEditorViewModel> Roles { get; set; }
    }

    // Used to display a single role with a checkbox, within a list structure:
    public class SelectRoleEditorViewModel
    {
        public SelectRoleEditorViewModel() { }
        public SelectRoleEditorViewModel(IdentityRole role)
        {
            this.RoleName = role.Name;
            this.RoleId = role.Id;
        }

        public bool Selected { get; set; }

        [Required]
        public string RoleName { get; set; }

        [Required]
        public string RoleId { get; set; }

    }
}