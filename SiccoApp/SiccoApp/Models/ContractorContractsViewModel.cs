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
    /// Todos los Contratos de un Contratista
    /// </summary>
    public class ContractorContractsViewModel
    {
        //ojo nuevo
        public int CustomerID { get; set; }
        public int ContractorID { get; set; }
        public string CompanyName { get; set; }
        public ICollection<Contract> Contracts { get; set; }

        public ContractorContractsViewModel(Contractor contractor)
        {
            this.CustomerID = contractor.CustomerID;
            this.ContractorID = contractor.ContractorID;
            this.CompanyName = contractor.CompanyName;
            this.Contracts = contractor.Contracts;
        }

    }


    public class EditContractorContractsViewModel
    {
        public EditContractorContractsViewModel() { }

        // Allow Initialization with an instance of ApplicationUser:
        public EditContractorContractsViewModel(Contract contract)
        {
            this.ContractID = contract.ContractID;
            this.ContractorID = contract.ContractorID;
            this.CustomerID = contract.CustomerID;
            this.ContractCode = contract.ContractCode;
            this.Description = contract.Description;
            this.StartDate= contract.StartDate;
            this.EndDate = contract.EndDate;

        }

        public int ContractID { get; set; }

        public int ContractorID { get; set; }

        //[Display(Name = "VehicleIdentificationNumber", ResourceType = typeof(Resources.Resources))]
        //[Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "VehicleIdentificationNumber_Required")]
        //[StringLength(50, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "VehicleIdentificationNumber_Long")]
        //public string IdentificationNumber { get; set; }

        public int CustomerID { get; set; }
        public string ContractCode { get; set; }

        [Display(Name = "ContractDescription", ResourceType = typeof(Resources.Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ContractDescription_Required")]
        [StringLength(250, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ContractDescription_Long")]
        public string Description { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "ContractStartDate", ResourceType = typeof(Resources.Resources))]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> StartDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "ContractEndDate", ResourceType = typeof(Resources.Resources))]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> EndDate { get; set; }

        // Return a pre-poulated instance of Contract:
        public Contract GetContract()
        {
            var contract = new Contract()
            {
                ContractID = this.ContractID,
                ContractorID = this.ContractorID,
                CustomerID = this.CustomerID,
                Description = this.Description,
                ContractCode = this.ContractCode,
                StartDate = this.StartDate,
                EndDate = this.EndDate
            };

            return contract;
        }

    }

}
