using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiccoApp.Persistence;


namespace SiccoApp.Models
{

    public class EmployeeContractViewModel
    {
        public int EmployeeContractID { get; set; }
        public int EmployeeID { get; set; }
        public int ContractID { get; set; }

        public string IdentificationNumber { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }

        public string ContractCode { get; set; }

        public EmployeeContractViewModel(EmployeeContract employeeContract)
        {
            this.EmployeeContractID = employeeContract.EmployeeContractID;
            this.EmployeeID = employeeContract.EmployeeID;
            this.ContractID = employeeContract.ContractID;
            this.IdentificationNumber = employeeContract.Employee.IdentificationNumber;
            this.LastName = employeeContract.Employee.LastName;
            this.FirstName = employeeContract.Employee.FirstName;
            this.ContractCode = employeeContract.Contract.ContractCode;
        }

    }
}
