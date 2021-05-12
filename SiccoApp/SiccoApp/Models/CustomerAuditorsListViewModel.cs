using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SiccoApp.Persistence;
using System.ComponentModel.DataAnnotations;

namespace SiccoApp.Models
{
    public class CustomerAuditorsListViewModel
    {
        public int CustomerID { get; set; }
        public ICollection<CustomerAuditorViewModel> CustomerAuditorUsers { get; set; }

        public CustomerAuditorsListViewModel(int customerID, ICollection<CustomerAuditor> customerAuditorList)
        {
            this.CustomerID = customerID;
            CustomerAuditorUsers = new List<CustomerAuditorViewModel>();

            foreach (var item in customerAuditorList)
            {
                CustomerAuditorUsers.Add(new CustomerAuditorViewModel(item));
            }
        }

    }
    public class EditCustomerAuditorUserViewModel
    {
        public EditCustomerAuditorUserViewModel() { }

        // Allow Initialization with an instance of ApplicationUser:
        public EditCustomerAuditorUserViewModel(CustomerAuditor user)
        {
            this.CustomerAuditorID = user.CustomerAuditorID;
            this.CustomerID = user.CustomerID;
            this.UserId = user.UserId;
            this.UserName = user.User.UserName;
            this.FirstName = user.User.FirstName;
            this.LastName = user.User.LastName;
            this.Email = user.User.Email;
        }

        [Required]
        [Display(Name = "CustomerAuditorID")]
        public int? CustomerAuditorID { get; set; }

        [Required]
        [Display(Name = "UserId")]
        public string UserId { get; set; }

        [Required]
        [Display(Name = "CustomerID")]
        public int CustomerID { get; set; }

        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }
    }
}