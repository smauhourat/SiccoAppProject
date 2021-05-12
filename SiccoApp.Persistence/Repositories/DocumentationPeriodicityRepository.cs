using SiccoApp.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SiccoApp.Persistence
{
    public class DocumentationPeriodicityRepository : IDocumentationPeriodicityRepository
    {
        private SiccoAppContext db = new SiccoAppContext();
        private ILogger log = null;

        public DocumentationPeriodicityRepository(ILogger logger)
        {
            log = logger;
        }

        public List<DocumentationPeriodicity> DocumentationPeriodicitys()
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                var result = db.DocumentationPeriodicitys.ToList();

                timespan.Stop();
                log.TraceApi("SQL Database", "DocumentationPeriodicityRepository.DocumentationPeriodicitys", timespan.Elapsed);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in DocumentationPeriodicityRepository.DocumentationPeriodicitys()");
                throw;
            }
        }

        public async Task<List<DocumentationPeriodicity>> DocumentationPeriodicitysAsync()
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                var result = await db.DocumentationPeriodicitys.ToListAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "DocumentationPeriodicityRepository.DocumentationPeriodicitysAsync", timespan.Elapsed);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in DocumentationPeriodicityRepository.DocumentationPeriodicitysAsync()");
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
