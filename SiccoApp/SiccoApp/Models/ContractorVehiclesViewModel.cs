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
    /// Todos los Vehiculos de un Contratista
    /// </summary>
    public class ContractorVehiclesViewModel
    {
        public int ContractorID { get; set; }
        public string CompanyName { get; set; }
        public ICollection<Vehicle> Vehicles { get; set; }

        public ContractorVehiclesViewModel(Contractor contractor)
        {
            this.ContractorID = contractor.ContractorID;
            this.CompanyName = contractor.CompanyName;
            this.Vehicles = contractor.Vehicles;
        }
    }

    public class EditContractorVehiclesViewModel
    {
        public EditContractorVehiclesViewModel() { }

        // Allow Initialization with an instance of ApplicationUser:
        public EditContractorVehiclesViewModel(Vehicle vehicle)
        {
            this.VehicleID = vehicle.VehicleID;
            this.ContractorID = vehicle.ContractorID;
            this.IdentificationNumber = vehicle.IdentificationNumber;
            this.Description = vehicle.Description;
            this.CreationDate = vehicle.CreationDate;
            this.CreationUser = vehicle.CreationUser;
            this.ModifiedDate = vehicle.ModifiedDate;
            this.ModifiedUser = vehicle.ModifiedUser;

        }

        public int VehicleID { get; set; }

        public int ContractorID { get; set; }

        [Display(Name = "VehicleIdentificationNumber", ResourceType = typeof(Resources.Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "VehicleIdentificationNumber_Required")]
        [StringLength(50, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "VehicleIdentificationNumber_Long")]
        public string IdentificationNumber { get; set; }

        [Display(Name = "VehicleDescription", ResourceType = typeof(Resources.Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "VehicleDescription_Required")]
        [StringLength(250, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "VehicleDescription_Long")]
        public string Description { get; set; }

        public Nullable<System.DateTime> CreationDate { get; set; }
        public string CreationUser { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string ModifiedUser { get; set; }

        // Return a pre-poulated instance of Vehicle:
        public Vehicle GetVehicle()
        {
            var vehicle = new Vehicle()
            {
                VehicleID = this.VehicleID,
                ContractorID = this.ContractorID,
                IdentificationNumber = this.IdentificationNumber,
                Description = this.Description,
                CreationDate = this.CreationDate,
                CreationUser = this.CreationUser,
                ModifiedDate = this.ModifiedDate,
                ModifiedUser = this.ModifiedUser 
            };

            return vehicle;
        }

    }
}
