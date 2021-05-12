using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiccoApp.Persistence;


namespace SiccoApp.Models
{

    public class VehicleContractViewModel
    {
        public int VehicleContractID { get; set; }
        public int VehicleID { get; set; }
        public int ContractID { get; set; }

        public string IdentificationNumber { get; set; }
        public string Description { get; set; }
        public string ContractCode { get; set; }

        public VehicleContractViewModel(VehicleContract vehicleContract)
        {
            this.VehicleContractID = vehicleContract.VehicleContractID;
            this.VehicleID = vehicleContract.VehicleID;
            this.ContractID = vehicleContract.ContractID;
            this.IdentificationNumber = vehicleContract.Vehicle.IdentificationNumber;
            this.Description = vehicleContract.Vehicle.Description;
            this.ContractCode = vehicleContract.Contract.ContractCode;
        }

    }
}
