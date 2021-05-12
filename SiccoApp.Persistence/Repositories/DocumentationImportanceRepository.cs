using SiccoApp.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SiccoApp.Persistence
{
    public class DocumentationImportanceRepository : IDocumentationImportanceRepository
    {
        private SiccoAppContext db = new SiccoAppContext();
        private ILogger log = null;

        public DocumentationImportanceRepository(ILogger logger)
        {
            log = logger;
        }

        public List<DocumentationImportance> DocumentationImportances()
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                var result = db.DocumentationImportances.ToList();

                timespan.Stop();
                log.TraceApi("SQL Database", "DocumentationImportanceRepository.DocumentationImportances", timespan.Elapsed);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in DocumentationImportanceRepository.DocumentationImportances()");
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
