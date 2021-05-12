using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SiccoApp.Persistence
{
    public interface IEntityTypeRepository : IDisposable
    {
        List<EntityType> EntityTypes();
        Task<List<EntityType>> EntityTypesAsync();
    }
}
