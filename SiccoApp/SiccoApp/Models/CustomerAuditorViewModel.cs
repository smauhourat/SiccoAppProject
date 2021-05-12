using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiccoApp.Persistence;
using System.ComponentModel.DataAnnotations;

namespace SiccoApp.Models
{
    public class CustomerAuditorViewModel
    {
        public int? CustomerAuditorID { get; set; }
        public int CustomerID { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public CustomerAuditorViewModel(CustomerAuditor customerAuditor)
        {
            this.CustomerAuditorID = customerAuditor.CustomerAuditorID;
            this.CustomerID = customerAuditor.CustomerID;
            this.UserId = customerAuditor.UserId;
            this.User = customerAuditor.User;
            this.UserName = customerAuditor.User.UserName;
            this.FirstName = customerAuditor.User.FirstName;
            this.LastName = customerAuditor.User.LastName;
            this.Email = customerAuditor.User.Email;

        }
    }
}