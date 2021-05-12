using SiccoApp.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SiccoApp.Persistence
{
    public class LocalizationRepository : ILocalizationRepository
    {
        private SiccoAppContext db = new SiccoAppContext();
        private ILogger log = null;

        public LocalizationRepository(ILogger logger)
        {
            log = logger;
        }

        public List<Country> Countries()
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                var result = db.Countries.ToList();

                timespan.Stop();
                log.TraceApi("SQL Database", "LocalizationRepository.Countries", timespan.Elapsed);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in LocalizationRepository.Countries()");
                throw;
            }
        }

        public async Task<List<Country>> CountriesAsync()
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                var result = await db.Countries.ToListAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "LocalizationRepository.CountriesAsync", timespan.Elapsed);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in LocalizationRepository.CountriesAsync()");
                throw;
            }
        }

        public List<State> States()
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                var result = db.States.ToList();

                timespan.Stop();
                log.TraceApi("SQL Database", "LocalizationRepository.States", timespan.Elapsed);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in LocalizationRepository.States()");
                throw;
            }
        }

        public List<State> States(int? countryId)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                var result = db.States.Where(s => s.CountryID == countryId).ToList();

                timespan.Stop();
                log.TraceApi("SQL Database", "LocalizationRepository.States", timespan.Elapsed);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in LocalizationRepository.States()");
                throw;
            }
        }

        public async Task<List<State>> StatesAsync()
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                var result = await db.States.ToListAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "LocalizationRepository.StatesAsync", timespan.Elapsed);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in LocalizationRepository.StatesAsync()");
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
