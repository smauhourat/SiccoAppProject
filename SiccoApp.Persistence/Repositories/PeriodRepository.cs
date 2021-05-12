using SiccoApp.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SiccoApp.Persistence
{
    public class PeriodRepository : IPeriodRepository, IDisposable
    {
        private SiccoAppContext db = new SiccoAppContext();
        private ILogger log = null;

        public PeriodRepository(ILogger logger)
        {
            log = logger;
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

        public async Task<List<Period>> FindPeriodsAsync()
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                var result = await db.Periods
                    .OrderByDescending(t => t.PeriodYear).OrderByDescending(t => t.PeriodID).ToListAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "PeriodRepository.FindPeriodsAsync", timespan.Elapsed);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in PeriodRepository.FindPeriodsAsync()");
                throw;
            }
        }
    }
}
