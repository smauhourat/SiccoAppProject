using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SiccoApp.Persistence
{
    public interface IDocumentationPeriodicityRepository : IDisposable
    {
        List<DocumentationPeriodicity> DocumentationPeriodicitys();
        Task<List<DocumentationPeriodicity>> DocumentationPeriodicitysAsync();
    }
}
