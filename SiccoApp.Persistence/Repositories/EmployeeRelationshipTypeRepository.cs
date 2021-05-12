using SiccoApp.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SiccoApp.Persistence
{
    public class EmployeeRelationshipTypeRepository : IEmployeeRelationshipTypeRepository
    {
        private SiccoAppContext db = new SiccoAppContext();
        private ILogger log = null;

        public EmployeeRelationshipTypeRepository(ILogger logger)
        {
            log = logger;
        }

        public List<EmployeeRelationshipType> EmployeeRelationshipTypes()
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                var result = db.EmployeeRelationshipTypes.ToList();

                timespan.Stop();
                log.TraceApi("SQL Database", "EmployeeRelationshipTypeRepository.EmployeeRelationshipTypes", timespan.Elapsed);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in EmployeeRelationshipTypeRepository.EmployeeRelationshipTypes()");
                throw;
            }
        }

        public async Task<List<EmployeeRelationshipType>> EmployeeRelationshipTypesAsync()
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                var result = await db.EmployeeRelationshipTypes.ToListAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "EmployeeRelationshipTypeRepository.EmployeeRelationshipTypesAsync", timespan.Elapsed);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in EmployeeRelationshipTypeRepository.EmployeeRelationshipTypesAsync()");
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
