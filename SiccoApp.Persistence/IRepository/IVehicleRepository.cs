using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SiccoApp.Persistence
{
    public interface IVehicleRepository : IDisposable
    {
        Task<List<Vehicle>> FindVehiclesAsync(int contractorID);
        Task<Vehicle> FindVehicleByIDAsync(int vehicleID);

        Task CreateAsync(Vehicle vehicleToAdd);
        Task DeleteAsync(int vehicleID);
        Task UpdateAsync(Vehicle vehicleToSave);

        Task DeleteContract(int vehicleID, int contractID);
        Task<VehicleContract> GetVehicleContract(int vehicleID, int contractID);
        List<Vehicle> UnAssignedVehicles(int contractID, int contractorID);
        Task CreateContractAsync(VehicleContract vehicleContract);

        Task GenerateContractorVehicleRequirements(int contractorID, int contractID, int vehicleID);
    }
}
