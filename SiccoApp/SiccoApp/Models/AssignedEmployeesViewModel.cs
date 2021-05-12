using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using SiccoApp.Persistence;

namespace SiccoApp.Models
{
    public class AssignedEmployeesViewModel
    {
        public int ContractorID { get; set; }
        public int ContractID { get; set; }
        public string ContractCode { get; set; }
        public string ContractDescription { get; set; }
        public IEnumerable<EmployeeContract> EmployeesContracts { get; set; }

        public AssignedEmployeesViewModel(Contract contract, List<EmployeeContract> employeesContracts)
        {
            this.ContractorID = contract.ContractorID;
            this.ContractID = contract.ContractID;
            this.ContractCode = contract.ContractCode;
            this.ContractDescription = contract.Description;
            this.EmployeesContracts = employeesContracts;
        }
    }

    public class EditAssignEmployeeViewModel
    {
        public int ContractorID { get; set; }
        public int EmployeeContractID { get; set; }

        [Display(Name = "EtyEmployee", ResourceType = typeof(Resources.Resources))]
        public int EmployeeID { get; set; }

        public int ContractID { get; set; }


        public EditAssignEmployeeViewModel() { }

        public EditAssignEmployeeViewModel(EmployeeContract employeeContract)
        {
            this.ContractorID = employeeContract.Contract.ContractorID;
            this.EmployeeContractID = employeeContract.EmployeeContractID;
            this.EmployeeID = employeeContract.EmployeeID;
            this.ContractID = employeeContract.ContractID;
            
            //employeeContract.Contract.ContractorID 
            //this.DocumentationDesc = docuBusinessType.Documentation.DocumentationCode + " - (" + docuBusinessType.Documentation.Description + ")";
        }

        public EmployeeContract GetContract()
        {
            var contract = new EmployeeContract()
            {
                EmployeeContractID = this.EmployeeContractID,
                EmployeeID = this.EmployeeID,
                ContractID  = this.ContractID
            };
            return contract;
        }
    }
}
