using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SiccoApp.Persistence
{
    public interface IDocumentationRepository : IDisposable
    {
        Task<List<Documentation>> FindDocumentationsAsync(int customerID);
        Task<Documentation> FindDocumentationByIDAsync(int documentationID);
        Task CreateAsync(Documentation documentationToAdd);
        Task DeleteAsync(int documentationID);
        Task UpdateAsync(Documentation documentationToSave);
        List<Documentation> Documentations(int customerID);
        List<Documentation> UnAssignedDocumentations(int businessTypeID, int customerID);
        Task<List<DocumentationResume>> FindDocumentationResumeByCustomerContractorAsync(int customerID, int contractorID);
    }
}
