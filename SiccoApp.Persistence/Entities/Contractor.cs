using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace SiccoApp.Persistence
{
    [MetadataType(typeof(ContractorMetaData))]
    public class Contractor : IValidatableObject
    {
        public int ContractorID { get; set; }
        public int CustomerID { get; set; }
        public string CompanyName { get; set; }
        public string TaxIdNumber { get; set; }

        public string FullCompanyName { get { return CompanyName + " (" + TaxIdNumber + ")"; } }
        public Nullable<int> CountryID { get; set; }
        public Country Country { get; set; }
        public Nullable<int> StateID { get; set; }
        public State State { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string EmergencyPhoneNumber { get; set; }
        public string Email { get; set; }
        public Nullable<byte> Active { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public string CreationUser { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string ModifiedUser { get; set; }
        public int BusinessTypeID { get; set; }

        [Display(Name = "EtyBusinessType", ResourceType = typeof(Resources.Resources))]
        public virtual BusinessType BusinessType { get; set; }

        public virtual ICollection<Contract> Contracts { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<Vehicle> Vehicles { get; set; }

        public virtual ICollection<ContractorUser> Users { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var property = new[] { "TaxIdNumber" };

            if (CompanyName.Contains("aaaaaaaasdasdasdasd") && TaxIdNumber == "1")
            {
                yield return new ValidationResult("Regla boba 1 para test en Tax", property);
            }
        }
    }
}
