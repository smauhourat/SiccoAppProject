using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SiccoApp.Persistence
{
    public interface IEmployeeRepository : IDisposable 
    {
        Task<List<Employee>> FindEmployeesAsync(int contractorID);
        Task<Employee> FindEmployeeByIDAsync(int employeeID);

        Task CreateAsync(Employee employeeToAdd);
        Task DeleteAsync(int employeeID);
        Task UpdateAsync(Employee employeeToSave);

        Task DeleteContract(int employeeID, int contractID);
        Task<EmployeeContract> GetEmployeeContract(int employeeID, int contractID);
        List<Employee> UnAssignedEmployees(int contractID, int contractorID);
        Task CreateContractAsync(EmployeeContract employeeContract);

        Task GenerateContractorEmployeeRequirements(int contractorID, int contractID, int employeeID);
    }
}
