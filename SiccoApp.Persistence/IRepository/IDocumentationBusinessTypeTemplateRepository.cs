using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SiccoApp.Persistence
{
    public interface IDocumentationBusinessTypeTemplateRepository : IDisposable
    {
        Task<List<DocumentationBusinessTypeTemplate>> FindDocumentationBusinessTypeTemplatesByBTAsync(int businessTypeTemplateID);
        Task<DocumentationBusinessTypeTemplate> FindDocumentationBusinessTypeTemplatesByIDAsync(int documentationBusinessTypeTemplateID);
        Task CreateAsync(DocumentationBusinessTypeTemplate documentToAdd);
        Task DeleteAsync(int documentationBusinessTypeTemplateID);
        Task UpdateAsync(DocumentationBusinessTypeTemplate documentToSave);
    }
}
