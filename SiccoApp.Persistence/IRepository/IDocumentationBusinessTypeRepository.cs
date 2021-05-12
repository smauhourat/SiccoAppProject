using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SiccoApp.Persistence
{
    public interface IDocumentationBusinessTypeRepository : IDisposable
    {
        List<DocumentationBusinessType> DocumentationBusinessTypes();
        Task<List<DocumentationBusinessType>> FindDocumentationBusinessTypesByBTAsync(int businessTypeID);
        Task<DocumentationBusinessType> FindDocumentationBusinessTypesByIDAsync(int documentationBusinessTypeID);
        Task CreateAsync(DocumentationBusinessType documentToAdd);
        Task DeleteAsync(int documentationBusinessTypeID);
        Task UpdateAsync(DocumentationBusinessType documentToSave);
    }
}
