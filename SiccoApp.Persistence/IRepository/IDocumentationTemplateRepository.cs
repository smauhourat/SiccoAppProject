using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SiccoApp.Persistence
{
    public interface IDocumentationTemplateRepository : IDisposable
    {
        Task<List<DocumentationTemplate>> FindDocumentationTemplatesAsync();

        Task<DocumentationTemplate> FindDocumentationTemplateByIDAsync(int documentationTemplateID);

        Task CreateAsync(DocumentationTemplate documentationTemplateToAdd);

        Task DeleteAsync(int documentationTemplateID);

        Task UpdateAsync(DocumentationTemplate documentationTemplateToSave);

        List<DocumentationTemplate> DocumentationTemplates();

        List<DocumentationTemplate> UnAssignedDocumentationTemplates(int businessTypeTemplateID);
    }
}
