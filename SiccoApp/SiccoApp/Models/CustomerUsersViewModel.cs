using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiccoApp.Persistence;

using System.ComponentModel.DataAnnotations;

namespace SiccoApp.Models
{
    /// <summary>
    /// Todos los Usuarios de un Cliente
    /// </summary>
    public class CustomerUsersViewModel
    {
        public int CustomerID { get; set; }
        public string CompanyName { get; set; }
        public virtual ICollection<CustomerUser> Users { get; set; }

        public CustomerUsersViewModel(Customer customer)
        {
            this.CustomerID = customer.CustomerID;
            this.CompanyName = customer.CompanyName;
            //OJOOO
            this.Users = customer.Users;
        }
    }

    public class EditCustomerUserViewModel
    {
        public EditCustomerUserViewModel() { }

        public int CustomerID { get; set; }

        // Allow Initialization with an instance of ApplicationUser:
        public EditCustomerUserViewModel(CustomerUser user)
        {
            this.UserName = user.UserName;
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;
            this.Email = user.Email;
        }

        [Required]
        [Display(Name = "UsernameLogin", ResourceType = typeof(Resources.Resources))]
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
}
