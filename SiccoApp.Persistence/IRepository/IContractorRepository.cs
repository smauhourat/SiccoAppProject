using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SiccoApp.Persistence
{
    public interface IContractorRepository : IDisposable
    {
        Task<List<Contractor>> FindContractorsAsync();
        List<Contractor> FindContractorsByCustomerID(int customerID);
        Task<List<Contractor>> FindContractorsByCustomerIDAsync(int customerID);
        Task<List<Contractor>> FindContractorsAsync(int customerID);
        Task<Contractor> FindContractorsByIDAsync(int contractorID);
        Task<Contractor> FindContractorsByIDAsync(int contractorID, int customerID);

        Task CreateAsync(Contractor contractorToAdd);
        Task DeleteAsync(int contractorID);
        Task UpdateAsync(Contractor contractorToSave);

        Task GenerateContractorRequirements(int contractorID, int contractID);
        Task GenerateContractorRequirementsAll(int contractorID, int contractID);
    }
}
