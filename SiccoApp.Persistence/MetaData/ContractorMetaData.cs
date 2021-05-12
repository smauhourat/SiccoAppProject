using System;
using System.ComponentModel.DataAnnotations;

namespace SiccoApp.Persistence
{
    class ContractorMetaData
    {
        public int ContractorID { get; set; }

        [Display(Name = "CompanyName_Name", ResourceType = typeof(Resources.Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "CompanyName_Required")]
        [StringLength(150, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "CompanyName_Long")]
        public string CompanyName { get; set; }

        [Display(Name = "TaxIdNumber_Name", ResourceType = typeof(Resources.Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "TaxIdNumber_Required")]
        [StringLength(20, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "TaxIdNumber_Long")]
        public string TaxIdNumber { get; set; }

        public Nullable<int> CountryID { get; set; }
        [Display(Name = "Country", ResourceType = typeof(Resources.Resources))]
        public Country Country { get; set; }

        public Nullable<int> StateID { get; set; }
        [Display(Name = "State", ResourceType = typeof(Resources.Resources))]
        public State State { get; set; }

        [Display(Name = "City", ResourceType = typeof(Resources.Resources))]
        public string City { get; set; }

        [Display(Name = "Address", ResourceType = typeof(Resources.Resources))]
        public string Address { get; set; }

        [Display(Name = "PhoneNumber_Name", ResourceType = typeof(Resources.Resources))]
        [StringLength(20, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PhoneNumber_Long")]
        public string PhoneNumber { get; set; }

        [Display(Name = "EmergPhoneNumber_Name", ResourceType = typeof(Resources.Resources))]
        [StringLength(20, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PhoneNumber_Long")]
        public string EmergencyPhoneNumber { get; set; }

        [Display(Name = "EtyBusinessType", ResourceType = typeof(Resources.Resources))]
        public int BusinessTypeID { get; set; }

    }
}
