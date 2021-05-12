using System;
using System.ComponentModel.DataAnnotations;

namespace SiccoApp.Persistence
{
    class VehicleMetaData
    {
        [Display(Name = "VehicleIdentificationNumber", ResourceType = typeof(Resources.Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "VehicleIdentificationNumber_Required")]
        [StringLength(50, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "VehicleIdentificationNumber_Long")]
        public string IdentificationNumber { get; set; }

        [Display(Name = "VehicleDescription", ResourceType = typeof(Resources.Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "VehicleDescription_Required")]
        [StringLength(250, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "VehicleDescription_Long")]
        public string Description { get; set; }
    }
}
