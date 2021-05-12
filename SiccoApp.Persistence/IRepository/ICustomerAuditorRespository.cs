using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SiccoApp.Persistence
{ 
    public interface ICustomerAuditorRespository : IDisposable
    {
        Task<List<ApplicationUser>> FindCustomerAuditorsAsync();
        Task<List<CustomerAuditor>> FindCustomerAuditorsByCustomerAsync(int? customerID);
        Task<List<CustomerAuditor>> FindCustomerAuditorsByUserIDAsync(string userID);
        Task<CustomerAuditor> FindByIdAsync(int customerAuditorID);

        void Create(CustomerAuditor customerAuditorToAdd);
        Task CreateAsync(CustomerAuditor customerAuditorToAdd);

        Task Delete(int customerAuditorID);
        Task DeleteAsync(int customerAuditorID);
        Task DeleteByUserIdAsync(string userId);

        void Update(CustomerAuditor customerAuditor);

        void SetRole(string UserId);
    }
}
