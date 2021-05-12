using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiccoApp.Persistence
{
    public interface IContractRepository : IDisposable
    {
        Task<List<EmployeeContract>> FindEmployeeContractsAsync(int contractID);
        Task<List<VehicleContract>> FindVehicleContractsAsync(int contractID);
        Task<List<Contract>> FindContractsAsync(int contractorID);
        Task<List<Contract>> FindContractsAsync(int customerID, int contractorID);
        Task<Contract> FindContractByIDAsync(int contractID);
        Task CreateAsync(Contract contractToAdd);
        Task DeleteAsync(int contractID);
        Task UpdateAsync(Contract contractToSave);
    }
}
