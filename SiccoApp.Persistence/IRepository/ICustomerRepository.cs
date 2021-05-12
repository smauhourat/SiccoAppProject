using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SiccoApp.Persistence
{
    public interface ICustomerRepository : IDisposable
    {
        Task<List<Customer>> FindCustomersAsync();
        Task<Customer> FindCustomerByIDAsync(int customerID);

        List<Customer> Customers();

        Task CreateAsync(Customer customerToAdd);
        //void Create(Customer customerToAdd);

        Task DeleteAsync(int customerID);
        
        Task UpdateAsync(Customer customerToSave);

        Task GenerateDocumentationMatrix(int customerID, bool forceDelete);

        Task DeleteDocumentationMatrix(int customerID);

        List<Customer> UnAssignedCustomers(string userID);
    }
}
