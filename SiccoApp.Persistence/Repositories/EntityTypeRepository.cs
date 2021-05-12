using SiccoApp.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SiccoApp.Persistence
{
    public class EntityTypeRepository : IEntityTypeRepository
    {
        private SiccoAppContext db = new SiccoAppContext();
        private ILogger log = null;

        public EntityTypeRepository(ILogger logger)
        {
            log = logger;
        }

        public List<EntityType> EntityTypes()
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                var result = db.EntityTypes.ToList();

                timespan.Stop();
                log.TraceApi("SQL Database", "EntityTypeRepository.EntityTypes", timespan.Elapsed);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in BusinessTypesRepository.BusinessTypes()");
                throw;
            }
        }

        public async Task<List<EntityType>> EntityTypesAsync()
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                var result = await db.EntityTypes.ToListAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "EntityTypeRepository.EntityTypesAsync", timespan.Elapsed);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in EntityTypeRepository.EntityTypesAsync()");
                throw;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Free managed resources
                if (db != null)
                {
                    db.Dispose();
                    db = null;
                }
            }
        }
    }
}
