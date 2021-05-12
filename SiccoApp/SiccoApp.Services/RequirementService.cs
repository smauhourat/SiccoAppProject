using SiccoApp.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiccoApp.Services
{
    public class RequirementService : IRequirementService
    {
        private IContractorRepository contractorRepository;
        private IContractRepository contractRepository;
        private IEmployeeRepository employeeRepository;
        private IVehicleRepository vehicleRepository;

        public RequirementService(IContractorRepository contractorRepo, IContractRepository contractRepo, IEmployeeRepository employeeRepo, IVehicleRepository vehicleRepo)
        {
            contractorRepository = contractorRepo;
            contractRepository = contractRepo;
            employeeRepository = employeeRepo;
            vehicleRepository = vehicleRepo;
        }

        public async Task GenerateContractorAllRequirements()
        {
            var contractors = await contractorRepository.FindContractorsAsync();

            foreach (var contractor in contractors)
            {
                var contracts = await contractRepository.FindContractsAsync(contractor.ContractorID);
                foreach (var contract in contracts)
                {
                    await contractorRepository.GenerateContractorRequirementsAll(contract.ContractorID, contract.ContractID);
                }
            }
        }

        #region IDisposable Support
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Free managed resources
                if (contractorRepository != null)
                {
                    contractorRepository.Dispose();
                    contractorRepository = null;
                }

                if (contractRepository != null)
                {
                    contractRepository.Dispose();
                    contractRepository = null;
                }

                if (employeeRepository != null)
                {
                    employeeRepository.Dispose();
                    employeeRepository = null;
                }

                if (vehicleRepository != null)
                {
                    vehicleRepository.Dispose();
                    vehicleRepository = null;
                }
            }
        }
        #endregion
    }
}
