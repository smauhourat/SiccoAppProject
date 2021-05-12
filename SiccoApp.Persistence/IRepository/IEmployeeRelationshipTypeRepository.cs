using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SiccoApp.Persistence
{
    public interface IEmployeeRelationshipTypeRepository : IDisposable 
    {
        List<EmployeeRelationshipType> EmployeeRelationshipTypes();
        Task<List<EmployeeRelationshipType>> EmployeeRelationshipTypesAsync();
    }
}
