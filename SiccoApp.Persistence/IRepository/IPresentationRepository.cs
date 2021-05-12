using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SiccoApp.Persistence
{
    public interface IPresentationRepository : IDisposable
    {
        Task<Presentation> FindByIdAsync(int presentationID);
        Task DeleteAsync(int presentationID);
        Task CreateAsync(Presentation presentationToAdd);
        Task TakeToAudit(Presentation presentation);
        Task Approve(Presentation presentation);
        Task Reject(Presentation presentation);
        Task<IList<Presentation>> FindByPeriodAsync(int periodID);
        //Task<List<CustomerAuditor>> FindAuditorsAsync(int presentationID);
    }
}
