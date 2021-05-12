using SiccoApp.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SiccoApp.Persistence
{
    public class DocumentationBusinessTypeTemplateRepository : IDocumentationBusinessTypeTemplateRepository
    {
        private SiccoAppContext db = new SiccoAppContext();
        private ILogger log = null;

        public DocumentationBusinessTypeTemplateRepository(ILogger logger)
        {
            log = logger;
        }

        public async Task<List<DocumentationBusinessTypeTemplate>> FindDocumentationBusinessTypeTemplatesByBTAsync(int businessTypeTemplateID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                var result = await db.DocumentationBusinessTypeTemplates
                    .Where(t => t.BusinessTypeTemplateID == businessTypeTemplateID)
                    .OrderByDescending(t => t.BusinessTypeTemplateID).ToListAsync();

                if (result !=  null) { 
                    foreach (DocumentationBusinessTypeTemplate item in result)
                    {
                        db.Entry(item).Reference(x => x.DocumentationTemplate).Load();
                    }
                }

                timespan.Stop();
                log.TraceApi("SQL Database", "DocumentationBusinessTypeTemplateRepository.FindDocumentationBusinessTypeTemplatesByBTAsync", timespan.Elapsed, "businessTypeTemplateID={0}", businessTypeTemplateID);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in DocumentationBusinessTypeTemplateRepository.FindDocumentationBusinessTypeTemplatesByBTAsync(businessTypeTemplateID={0})", businessTypeTemplateID);
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

        public async Task CreateAsync(DocumentationBusinessTypeTemplate documentToAdd)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.DocumentationBusinessTypeTemplates.Add(documentToAdd);
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "DocumentationBusinessTypeTemplateRepository.CreateAsync", timespan.Elapsed, "documentToAdd={0}", documentToAdd);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in DocumentationBusinessTypeTemplateRepository.CreateAsync(documentToAdd={0})", documentToAdd);
                throw;
            }
        }

        public async Task<DocumentationBusinessTypeTemplate> FindDocumentationBusinessTypeTemplatesByIDAsync(int documentationBusinessTypeTemplateID)
        {
            DocumentationBusinessTypeTemplate docuBusinessType = null;
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                docuBusinessType = await db.DocumentationBusinessTypeTemplates.FindAsync(documentationBusinessTypeTemplateID);

                timespan.Stop();
                log.TraceApi("SQL Database", "DocumentationBusinessTypeTemplateRepository.FindDocumentationBusinessTypeTemplatesByIDAsync", timespan.Elapsed, "documentationBusinessTypeTemplateID={0}", documentationBusinessTypeTemplateID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in DocumentationBusinessTypeTemplateRepository.FindDocumentationBusinessTypeTemplatesByIDAsync(documentationBusinessTypeTemplateID={0})", documentationBusinessTypeTemplateID);
                throw;
            }

            return docuBusinessType;
        }

        public async Task UpdateAsync(DocumentationBusinessTypeTemplate documentToSave)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.Entry(documentToSave).State = EntityState.Modified;
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "DocumentationBusinessTypeTemplateRepository.UpdateAsync", timespan.Elapsed, "documentToSave={0}", documentToSave);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in DocumentationBusinessTypeTemplateRepository.UpdateAsync(documentToSave={0})", documentToSave);
                throw;
            }
        }

        public async Task DeleteAsync(int documentationBusinessTypeTemplateID)
        {
            DocumentationBusinessTypeTemplate docuBusinessType = null;
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                docuBusinessType = await db.DocumentationBusinessTypeTemplates.FindAsync(documentationBusinessTypeTemplateID);
                db.DocumentationBusinessTypeTemplates.Remove(docuBusinessType);
                db.SaveChanges();

                timespan.Stop();
                log.TraceApi("SQL Database", "DocumentationBusinessTypeTemplateRepository.DeleteAsync", timespan.Elapsed, "documentationBusinessTypeTemplateID={0}", documentationBusinessTypeTemplateID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in DocumentationBusinessTypeTemplateRepository.DeleteAsync(documentationBusinessTypeTemplateID={0})", documentationBusinessTypeTemplateID);
                throw;
            }
        }

        //private void DeleteByBusinessTypeTemplate(int businessTypeTemplateID)
        //{
        //    Stopwatch timespan = Stopwatch.StartNew();

        //    try
        //    {
        //        var docBusinessTypeTemplates = db.DocumentationBusinessTypeTemplates
        //            .Where(t => t.BusinessTypeTemplateID == businessTypeTemplateID).ToList();

        //        foreach (DocumentationBusinessTypeTemplate item in docBusinessTypeTemplates)
        //        {
        //            db.DocumentationBusinessTypeTemplates.Remove(item);
        //        }
        //        db.SaveChanges();

        //        timespan.Stop();
        //        log.TraceApi("SQL Database", "DeleteByBusinessTypeTemplate.DeleteByBusinessTypeTemplate", timespan.Elapsed, "businessTypeTemplateID={0}", businessTypeTemplateID);
        //    }
        //    catch (Exception e)
        //    {
        //        log.Error(e, "Error in DeleteByBusinessTypeTemplate.DeleteByBusinessTypeTemplate()");
        //        throw;
        //    }
        //}
    }
}
