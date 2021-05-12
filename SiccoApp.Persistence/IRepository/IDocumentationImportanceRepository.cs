using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiccoApp.Persistence
{
    public interface IDocumentationImportanceRepository : IDisposable
    {
        List<DocumentationImportance> DocumentationImportances();

        //Task<DocumentationImportance> FindDocumentationImportanceByIDAsync(int documentationImportanceID);

        //Task CreateAsync(DocumentationImportance documentationImportanceIDToAdd);

        //Task DeleteAsync(int documentationImportanceID);

        //Task UpdateAsync(DocumentationImportance documentationImportanceIDToSave);
    }
}
