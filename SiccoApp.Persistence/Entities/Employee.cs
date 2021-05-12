using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace SiccoApp.Persistence
{
    [MetadataType(typeof(EmployeeMetaData))]
    public class Employee
    {
        public int EmployeeID { get; set; }
        public int ContractorID { get; set; }
        public int IdentificationNumberTypeID { get; set; }
        public string IdentificationNumber { get; set; }
        public string SocialSecurityNumber { get; set; }
        public int EmployeeRelationshipTypeID { get; set; }
        public virtual EmployeeRelationshipType EmployeeRelationshipType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string MaritalStatus { get; set; }
        public string Gender { get; set; }
        public Nullable<System.DateTime> BirthDate { get; set; }
        public Nullable<int> CountryID { get; set; }
        public Nullable<int> StateID { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public Nullable<byte> Active { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public string CreationUser { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string ModifiedUser { get; set; }
        public bool Disabled { get; set; }
        public Nullable<DateTime> DisabledDate { get; set; }

        public string FullName { get { return LastName + ", " + FirstName + (String.IsNullOrEmpty(IdentificationNumber) ? "" : "(" + IdentificationNumber.ToString() + ")");  } }
    
        public virtual Contractor Contractor { get; set; }
        //public virtual ICollection<EmployeeContract> EmployeeContract { get; set; }
    }
}
