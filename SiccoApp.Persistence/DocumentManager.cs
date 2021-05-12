using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiccoApp.Persistence
{
    public static class DocumentManager
    {

        public static string GetFileName(Requirement requirement)
        {
            //< CUIT Contratista > -< Razon Social > -< Periodo > -< Identificador mas Descripcion del Recurso (1) > -< Descripcion Tipo Documentacion ><>

            var res = "";

            res = requirement.Contract.Contractor.TaxIdNumber + "-" +
                requirement.Contract.Contractor.CompanyName + "-" +
                requirement.PeriodID.ToString() + "-" +
                GetResourceIdentificator(requirement) + "-" +
                requirement.DocumentationBusinessType.Documentation.DocumentationCode;

            return res;
        }

        private static string GetResourceIdentificator(Requirement requirement)
        {
            var res = "";

            if (requirement.EmployeeContractID == null && requirement.VehicleContractID == null)
            {
                res = requirement.Contract.Contractor.TaxIdNumber + "-" +
                requirement.Contract.Contractor.CompanyName;
            }
            else
            {
                if (requirement.EmployeeContractID != null)
                {
                    res = requirement.EmployeeContract.Employee.IdentificationNumber.ToString() + "-" +
                        requirement.EmployeeContract.Employee.LastName + " " + requirement.EmployeeContract.Employee.FirstName;
                }
                else
                {
                    if (requirement.VehicleContractID != null)
                    {
                        res = requirement.VehicleContract.Vehicle.IdentificationNumber;
                    }
                }
            }

            if (res == "") throw new ArgumentException("Requirement object bad format");

            return res;
        }
    }
}
