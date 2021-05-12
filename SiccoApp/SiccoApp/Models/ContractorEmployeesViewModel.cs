using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using SiccoApp.Persistence;

namespace SiccoApp.Models
{
    /// <summary>
    /// Todos los Empleados de un Contratista
    /// </summary>
    public class ContractorEmployeesViewModel
    {
        public int ContractorID { get; set; }
        public string CompanyName { get; set; }
        //public ICollection<Employee> Employees2 { get; set; }
        public IEnumerable<EditContractorEmployeesViewModel> Employees { get; set; }

        public ContractorEmployeesViewModel(Contractor contractor)
        {
            this.ContractorID = contractor.ContractorID;
            this.CompanyName = contractor.CompanyName;
            //this.Employees2 = contractor.Employees;
            this.Employees = contractor.Employees.Select(item => new EditContractorEmployeesViewModel(item));
        }
    }

    public class EditContractorEmployeesViewModel
    {
        public EditContractorEmployeesViewModel() { }

        // Allow Initialization with an instance of ApplicationUser:
        public EditContractorEmployeesViewModel(Employee employee)
        {
            this.EmployeeID = employee.EmployeeID;
            this.ContractorID = employee.ContractorID;
            this.FirstName = employee.FirstName;
            this.Email = employee.Email;
            this.LastName = employee.LastName;
            this.IdentificationNumber = employee.IdentificationNumber;
            this.SocialSecurityNumber = employee.SocialSecurityNumber;
            this.EmployeeRelationshipTypeID = employee.EmployeeRelationshipTypeID;
            this.EmployeeRelationshipType = employee.EmployeeRelationshipType;
            this.PhoneNumber = employee.PhoneNumber;
            this.CreationDate = employee.CreationDate;
            this.CreationUser = employee.CreationUser;
            this.ModifiedDate = employee.ModifiedDate;
            this.ModifiedUser = employee.ModifiedUser;
            this.CompanyName = employee.Contractor.CompanyName;
        }

        public int EmployeeID { get; set; }

        public int ContractorID { get; set; }

        [Display(Name = "FullName", ResourceType = typeof(Resources.Resources))]
        public string FullName
        {
            get { return LastName + ", " + FirstName;  }
        }

        [Display(Name = "FirstName", ResourceType = typeof(Resources.Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "FirstName_Required")]
        [StringLength(50, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "FirstName_Long")]
        public string FirstName { get; set; }

        [Display(Name = "LastName", ResourceType = typeof(Resources.Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "LastName_Required")]
        [StringLength(50, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "LastName_Long")]
        public string LastName { get; set; }

        [Display(Name = "Email", ResourceType = typeof(Resources.Resources))]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "EmployeeIdentNumber_Required")]
        [Display(Name = "IdentificationNumber", ResourceType = typeof(Resources.Resources))]
        public string IdentificationNumber { get; set; }

        [Display(Name = "SocialSecurityNumber", ResourceType = typeof(Resources.Resources))]
        public string SocialSecurityNumber { get; set; }

        [Display(Name = "EmployeeRelationshipType", ResourceType = typeof(Resources.Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "EmployeeRelationshipType_Required")]
        public int EmployeeRelationshipTypeID { get; set; }

        //[Display(Name = "EmployeeRelationshipType", ResourceType = typeof(Resources.Resources))]
        //[Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "EmployeeRelationshipType_Required")]
        public EmployeeRelationshipType EmployeeRelationshipType { get; set; }


        [Display(Name = "PhoneNumber", ResourceType = typeof(Resources.Resources))]
        [Phone]
        public string PhoneNumber { get; set; }


        public Nullable<System.DateTime> CreationDate { get; set; }
        public string CreationUser { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string ModifiedUser { get; set; }

        public string QRCodeText { get { return EmployeeID.ToString() + "@" + FullName + "@" + IdentificationNumber.ToString() + "@" + CompanyName; } }

        public string QRCode { get; set; }

        [Display(Name = "CompanyName_Name", ResourceType = typeof(Resources.Resources))]
        public string CompanyName { get; set; }

        // Return a pre-poulated instance of Employee:
        public Employee GetEmployee()
        {
            var employee = new Employee()
            {
                EmployeeID = this.EmployeeID,
                ContractorID = this.ContractorID,
                FirstName = this.FirstName,
                LastName = this.LastName,
                Email = this.Email,
                IdentificationNumber = this.IdentificationNumber,
                SocialSecurityNumber = this.SocialSecurityNumber,
                EmployeeRelationshipTypeID = this.EmployeeRelationshipTypeID,
                EmployeeRelationshipType = this.EmployeeRelationshipType,
                PhoneNumber = this.PhoneNumber,
                CreationDate = this.CreationDate,
                CreationUser = this.CreationUser,
                ModifiedDate = this.ModifiedDate,
                ModifiedUser = this.ModifiedUser
            };
            return employee;
        }

    }

}
