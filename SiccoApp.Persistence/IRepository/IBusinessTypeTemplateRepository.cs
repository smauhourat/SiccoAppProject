using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiccoApp.Persistence
{
    public interface IBusinessTypeTemplateRepository : IDisposable
    {
        Task<List<BusinessTypeTemplate>> FindBusinessTypeTemplatesAsync();

        Task<BusinessTypeTemplate> FindBusinessTypeTemplateByIDAsync(int businessTypeTemplateID);

        Task CreateAsync(BusinessTypeTemplate businessTypeTemplateToAdd);

        Task DeleteAsync(int businessTypeTemplateID);

        Task UpdateAsync(BusinessTypeTemplate businessTypeTemplateToSave);
    }
}
