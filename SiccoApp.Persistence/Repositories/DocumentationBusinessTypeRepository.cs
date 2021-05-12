using SiccoApp.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SiccoApp.Persistence
{
    public class DocumentationBusinessTypeRepository : IDocumentationBusinessTypeRepository
    {
        private SiccoAppContext db = new SiccoAppContext();
        private ILogger log = null;

        public DocumentationBusinessTypeRepository(ILogger logger)
        {
            log = logger;
        }

        public List<DocumentationBusinessType> DocumentationBusinessTypes()
        {
            List<DocumentationBusinessType> docuBusinessTypes = null;
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                docuBusinessTypes = db.DocumentationBusinessTypes.ToList();

                timespan.Stop();
                log.TraceApi("SQL Database", "DocumentationBusinessTypeRepository.DocumentationBusinessTypes", timespan.Elapsed);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in DocumentationBusinessTypeRepository.DocumentationBusinessTypes()");
                throw;
            }

            return docuBusinessTypes;
        }

        public async Task<List<DocumentationBusinessType>> FindDocumentationBusinessTypesByBTAsync(int businessTypeID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                var result = await db.DocumentationBusinessTypes
                    .Where(t => t.BusinessTypeID == businessTypeID)
                    .OrderByDescending(t => t.BusinessTypeID).ToListAsync();

                if (result !=  null) { 
                    foreach (DocumentationBusinessType item in result)
                    {
                        db.Entry(item).Reference(x => x.Documentation).Load();
                    }
                }

                timespan.Stop();
                log.TraceApi("SQL Database", "DocumentationBusinessTypeRepository.FindDocumentationBusinessTypesByBTAsync", timespan.Elapsed, "businessTypeID={0}", businessTypeID);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in DocumentationBusinessTypeRepository.FindDocumentationBusinessTypesByBTAsync(businessTypeID={0})", businessTypeID);
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

        public async Task CreateAsync(DocumentationBusinessType documentToAdd)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.DocumentationBusinessTypes.Add(documentToAdd);
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "DocumentationBusinessTypeRepository.CreateAsync", timespan.Elapsed, "documentToAdd={0}", documentToAdd);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in DocumentationBusinessTypeRepository.CreateAsync(documentToAdd={0})", documentToAdd);
                throw;
            }
        }

        public async Task<DocumentationBusinessType> FindDocumentationBusinessTypesByIDAsync(int documentationBusinessTypeID)
        {
            DocumentationBusinessType docuBusinessType = null;
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                docuBusinessType = await db.DocumentationBusinessTypes.FindAsync(documentationBusinessTypeID);

                timespan.Stop();
                log.TraceApi("SQL Database", "DocumentationBusinessTypeRepository.FindDocumentationBusinessTypesByIDAsync", timespan.Elapsed, "documentationBusinessTypeID={0}", documentationBusinessTypeID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in DocumentationBusinessTypeRepository.FindDocumentationBusinessTypesByIDAsync(documentationBusinessTypeID={0})", documentationBusinessTypeID);
                throw;
            }

            return docuBusinessType;
        }

        public async Task UpdateAsync(DocumentationBusinessType documentToSave)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.Entry(documentToSave).State = EntityState.Modified;
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "DocumentationBusinessTypeRepository.UpdateAsync", timespan.Elapsed, "documentToSave={0}", documentToSave);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in DocumentationBusinessTypeRepository.UpdateAsync(documentToSave={0})", documentToSave);
                throw;
            }
        }

        public async Task DeleteAsync(int documentationBusinessTypeID)
        {
            DocumentationBusinessType docuBusinessType = null;
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                docuBusinessType = await db.DocumentationBusinessTypes.FindAsync(documentationBusinessTypeID);
                db.DocumentationBusinessTypes.Remove(docuBusinessType);
                db.SaveChanges();

                timespan.Stop();
                log.TraceApi("SQL Database", "DocumentationBusinessTypeRepository.DeleteAsync", timespan.Elapsed, "documentationBusinessTypeID={0}", documentationBusinessTypeID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in DocumentationBusinessTypeRepository.DeleteAsync(documentationBusinessTypeID={0})", documentationBusinessTypeID);
                throw;
            }
        }
    }
}
