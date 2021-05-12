using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using SiccoApp.Persistence;

namespace SiccoApp.Models
{
    public class AssignedVehiclesViewModel
    {
        public int ContractorID { get; set; }
        public int ContractID { get; set; }
        public string ContractCode { get; set; }
        public string ContractDescription { get; set; }
        public IEnumerable<VehicleContract> VehiclesContracts { get; set; }

        public AssignedVehiclesViewModel(Contract contract, List<VehicleContract> vehiclesContracts)
        {
            this.ContractorID = contract.ContractorID;
            this.ContractID = contract.ContractID;
            this.ContractCode = contract.ContractCode;
            this.ContractDescription = contract.Description;
            this.VehiclesContracts = vehiclesContracts;
        }
    }

    public class EditAssignVehicleViewModel
    {
        public int ContractorID { get; set; }
        public int VehicleContractID { get; set; }

        [Display(Name = "EtyVehicle", ResourceType = typeof(Resources.Resources))]
        public int VehicleID { get; set; }

        public int ContractID { get; set; }


        public EditAssignVehicleViewModel() { }

        public EditAssignVehicleViewModel(VehicleContract vehicleContract)
        {
            this.ContractorID = vehicleContract.Contract.ContractorID;
            this.VehicleContractID = vehicleContract.VehicleContractID;
            this.VehicleID = vehicleContract.VehicleID;
            this.ContractID = vehicleContract.ContractID;
        }

        public VehicleContract GetContract()
        {
            var contract = new VehicleContract()
            {
                VehicleContractID = this.VehicleContractID,
                VehicleID = this.VehicleID,
                ContractID  = this.ContractID
            };
            return contract;
        }
    }
}
