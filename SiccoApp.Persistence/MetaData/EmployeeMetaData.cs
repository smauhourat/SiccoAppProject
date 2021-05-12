using System;
using System.ComponentModel.DataAnnotations;

namespace SiccoApp.Persistence
{
    class EmployeeMetaData
    {
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "EmployeeIdentNumber_Required")]
        [Display(Name = "IdentificationNumber", ResourceType = typeof(Resources.Resources))]
        public string IdentificationNumber { get; set; }

        [Display(Name = "SocialSecurityNumber", ResourceType = typeof(Resources.Resources))]
        public string SocialSecurityNumber { get; set; }

        [Display(Name = "FirstName", ResourceType = typeof(Resources.Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "FirstName_Required")]
        [StringLength(50, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "FirstName_Long")]
        public string FirstName { get; set; }


        [Display(Name = "EmployeeRelationshipType", ResourceType = typeof(Resources.Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "EmployeeRelationshipType_Required")]
        public int EmployeeRelationshipTypeID { get; set; }

        [Display(Name = "LastName", ResourceType = typeof(Resources.Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "LastName_Required")]
        [StringLength(50, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "LastName_Long")]
        public string LastName { get; set; }

        [Display(Name = "Email", ResourceType = typeof(Resources.Resources))]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "PhoneNumber", ResourceType = typeof(Resources.Resources))]
        [Phone]
        public string PhoneNumber { get; set; }
    }

    public static class EmployeeRules
    {
        public static ValidationResult ValidateEmployee(Employee employee, ValidationContext validationContext)
        {
            
            //if (documentationTemplate.Description == "noononononono")
            //{
            //    return new ValidationResult("Regla boba 1 para test en Tax");
            //}

            return null;
        }
    }
}
