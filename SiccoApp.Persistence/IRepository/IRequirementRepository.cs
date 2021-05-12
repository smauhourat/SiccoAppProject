using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiccoApp.Persistence
{
    public interface IRequirementRepository : IDisposable
    {
        void GenerateByPeriodAsync(int customerID, int periodID, DateTime dueDate, string result);

        Task CreateAsync(Requirement requirementToAdd);
        Task UpdateAsync(Requirement requirementToSave);
        Task DeleteAsync(int requirementID);

        Task<Requirement> FindRequirementByIDAsync(int requirementID);

        Task<List<Requirement>> FindRequirementsByFilterAsync(int customerID, int contractorID, int contractID, int periodID, RequirementStatus requirementStatus, int entityTypeID);

        Task<List<Presentation>> FindPresentationsAsync(int requirementID);
        Task<List<Requirement>> FindRequirementsNextToExpireAsync();
    }
}
