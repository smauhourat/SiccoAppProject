using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SiccoApp.Persistence
{
    public interface IBusinessTypeRepository : IDisposable
    {
        List<BusinessType> BusinessTypes();
        List<BusinessType> BusinessTypes(int customerID);
        Task<List<BusinessType>> BusinessTypesAsync();

        Task<List<BusinessType>> FindBusinessTypesAsync(int customerID);
        Task<BusinessType> FindBusinessTypeByIDAsync(int businessTypeID);
        Task CreateAsync(BusinessType businessTypeToAdd);
        Task DeleteAsync(int businessTypeID);
        Task UpdateAsync(BusinessType businessTypeToSave);
    }
}
