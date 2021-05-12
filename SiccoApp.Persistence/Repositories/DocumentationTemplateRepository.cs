using SiccoApp.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SiccoApp.Persistence
{
    public class DocumentationTemplateRepository : IDocumentationTemplateRepository
    {
        private SiccoAppContext db = new SiccoAppContext();
        private ILogger log = null;

        public DocumentationTemplateRepository(ILogger logger)
        {
            log = logger;
        }

        public async Task CreateAsync(DocumentationTemplate documentationTemplateToAdd)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.DocumentationTemplates.Add(documentationTemplateToAdd);
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "DocumentationTemplateRepository.CreateAsync", timespan.Elapsed, "documentationTemplateToAdd={0}", documentationTemplateToAdd);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in DocumentationTemplateRepository.CreateAsync(documentationTemplateToAdd={0})", documentationTemplateToAdd);
                throw;
            }
        }

        private void DeleteDocumentationBusinessTypesTemplate(int documentationTemplateID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                var docBusinessTypeTemplates = db.DocumentationBusinessTypeTemplates
                    .Where(t => t.DocumentationTemplateID == documentationTemplateID).ToList();

                foreach (DocumentationBusinessTypeTemplate item in docBusinessTypeTemplates)
                {
                    db.DocumentationBusinessTypeTemplates.Remove(item);
                }
                db.SaveChanges();

                timespan.Stop();
                log.TraceApi("SQL Database", "DocumentationTemplateRepository.DeleteDocumentationBusinessTypesTemplate", timespan.Elapsed, "documentationTemplateID={0}", documentationTemplateID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in DocumentationTemplateRepository.DeleteDocumentationBusinessTypesTemplate()");
                throw;
            }
        }

        public async Task DeleteAsync(int documentationTemplateID)
        {
            DocumentationTemplate documentationTemplate = null;
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                DeleteDocumentationBusinessTypesTemplate(documentationTemplateID);

                documentationTemplate = await db.DocumentationTemplates.FindAsync(documentationTemplateID);
                db.DocumentationTemplates.Remove(documentationTemplate);
                db.SaveChanges();

                timespan.Stop();
                log.TraceApi("SQL Database", "DocumentationTemplateRepository.DeleteAsync", timespan.Elapsed, "documentationTemplateID={0}", documentationTemplateID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in DocumentationTemplateRepository.DeleteAsync(documentationTemplateID={0})", documentationTemplateID);
                throw;
            }
        }

        public async Task<DocumentationTemplate> FindDocumentationTemplateByIDAsync(int documentationTemplateID)
        {
            DocumentationTemplate documentationTemplate = null;
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                documentationTemplate = await db.DocumentationTemplates.FindAsync(documentationTemplateID);

                timespan.Stop();
                log.TraceApi("SQL Database", "DocumentationTemplateRepository.FindDocumentationTemplateByIDAsync", timespan.Elapsed, "documentationTemplateID={0}", documentationTemplateID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in DocumentationTemplateRepository.FindDocumentationTemplateByIDAsync(documentationTemplateID={0})", documentationTemplateID);
                throw;
            }

            return documentationTemplate;
        }

        public async Task<List<DocumentationTemplate>> FindDocumentationTemplatesAsync()
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                var result = await db.DocumentationTemplates
                    .OrderBy(t => t.DocumentationTemplateCode).ToListAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "DocumentationTemplateRepository.FindDocumentationTemplatesAsync", timespan.Elapsed);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in DocumentationTemplateRepository.FindDocumentationTemplatesAsync()");
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

        public async Task UpdateAsync(DocumentationTemplate documentationTemplateToSave)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.Entry(documentationTemplateToSave).State = EntityState.Modified;
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "DocumentationTemplateRepository.UpdateAsync", timespan.Elapsed, "documentationTemplateToSave={0}", documentationTemplateToSave);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in DocumentationTemplateRepository.UpdateAsync(documentationTemplateToSave={0})", documentationTemplateToSave);
                throw;
            }
        }

        public List<DocumentationTemplate> DocumentationTemplates()
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                var result = db.DocumentationTemplates.ToList();

                timespan.Stop();
                log.TraceApi("SQL Database", "DocumentationTemplateRepository.DocumentationTemplates", timespan.Elapsed);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in DocumentationTemplateRepository.DocumentationTemplates()");
                throw;
            }
        }

        public List<DocumentationTemplate> UnAssignedDocumentationTemplates(int businessTypeTemplateID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                var result = db.DocumentationTemplates.ToList();

                var docBusiness = db.DocumentationBusinessTypeTemplates
                    .Where(t => t.BusinessTypeTemplateID == businessTypeTemplateID).ToList();

                foreach (DocumentationBusinessTypeTemplate item in docBusiness)
                {
                    //item.DocumentationTemplate 
                    if (result.Contains(item.DocumentationTemplate))
                        result.Remove(item.DocumentationTemplate);
                }

                timespan.Stop();
                log.TraceApi("SQL Database", "DocumentationTemplateRepository.DocumentationTemplates", timespan.Elapsed);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in DocumentationTemplateRepository.DocumentationTemplates()");
                throw;
            }
        }
    }
}
