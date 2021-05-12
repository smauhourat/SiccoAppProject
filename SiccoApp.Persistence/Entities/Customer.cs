using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace SiccoApp.Persistence
{
    //[CustomValidation(typeof(CustomerRules), "ValidateCustomer")]
    [MetadataType(typeof(CustomerMetaData))]
    public class Customer : IValidatableObject 
    {
        public int CustomerID { get; set; }
        public string CompanyName { get; set; }
        public string TaxIdNumber { get; set; }
        public string FullCompanyName { get { return CompanyName + " (" + TaxIdNumber + ")"; } }
        public Nullable<int> CountryID { get; set; }
        public virtual Country Country { get; set; }
        public int? StateID { get; set; }
        public virtual State State { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public Nullable<byte> Active { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public string CreationUser { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string ModifiedUser { get; set; }

        public virtual ICollection<CustomerUser> Users { get; set; }

        public ICollection<Documentation> Documentations { get; set; }
        public ICollection<BusinessType> BusinessTypes { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            //throw new NotImplementedException();

            var property = new[] { "TaxIdNumber" };

            if (CompanyName.Contains("CCCCCCCCCCC") && TaxIdNumber == "1")
            {
                yield return new ValidationResult("Regla boba 1 para test en Tax", property);
            }

            //if (City != null) { 
            //    if (City.Contains("a") && TaxIdNumber == "1")
            //    {
            //        yield return new ValidationResult("Regla boba 2 para test en Tax", property);
            //    }
            //}

        }
    }
}
