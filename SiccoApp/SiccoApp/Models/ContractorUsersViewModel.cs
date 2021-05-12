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
    public class ContractorUsersViewModel
    {
        public int ContractorID { get; set; }
        public string CompanyName { get; set; }
        public ICollection<ContractorUser> Users { get; set; }

        public ContractorUsersViewModel(Contractor contractor)
        {
            this.ContractorID = contractor.ContractorID;
            this.CompanyName = contractor.CompanyName;
            this.Users = contractor.Users;
        }
    }

    public class EditContractorUserViewModel
    {
        public EditContractorUserViewModel() { }

        // Allow Initialization with an instance of ApplicationUser:
        public EditContractorUserViewModel(ContractorUser user)
        {
            this.ContractorID = user.ContractorID;
            this.UserName = user.UserName;
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;
            this.Email = user.Email;
        }

        [Required]
        [Display(Name = "ContractorID")]
        public int ContractorID { get; set; }

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
