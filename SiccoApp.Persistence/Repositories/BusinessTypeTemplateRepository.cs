using SiccoApp.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SiccoApp.Persistence
{
    public class BusinessTypeTemplateRepository : IBusinessTypeTemplateRepository
    {
        private SiccoAppContext db = new SiccoAppContext();
        private ILogger log = null;

        public BusinessTypeTemplateRepository(ILogger logger)
        {
            log = logger;
        }

        public async Task CreateAsync(BusinessTypeTemplate businessTypeTemplateToAdd)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.BusinessTypeTemplates.Add(businessTypeTemplateToAdd);
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "BusinessTypeTemplateRepository.CreateAsync", timespan.Elapsed, "businessTypeTemplateToAdd={0}", businessTypeTemplateToAdd);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in BusinessTypeTemplateRepository.CreateAsync(businessTypeTemplateToAdd={0})", businessTypeTemplateToAdd);
                throw;
            }
        }

        public async Task DeleteAsync(int businessTypeTemplateID)
        {
            BusinessTypeTemplate businessTypeTemplate = null;
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                DeleteDocumentationBusinessTypesTemplate(businessTypeTemplateID);

                businessTypeTemplate = await db.BusinessTypeTemplates.FindAsync(businessTypeTemplateID);
                db.BusinessTypeTemplates.Remove(businessTypeTemplate);
                db.SaveChanges();

                timespan.Stop();
                log.TraceApi("SQL Database", "BusinessTypeTemplateRepository.DeleteAsync", timespan.Elapsed, "businessTypeTemplateID={0}", businessTypeTemplateID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in BusinessTypeTemplateRepository.DeleteAsync(businessTypeTemplateID={0})", businessTypeTemplateID);
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

        public async Task<BusinessTypeTemplate> FindBusinessTypeTemplateByIDAsync(int businessTypeTemplateID)
        {
            BusinessTypeTemplate businessTypeTemplate = null;
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                businessTypeTemplate = await db.BusinessTypeTemplates.FindAsync(businessTypeTemplateID);

                timespan.Stop();
                log.TraceApi("SQL Database", "BusinessTypeTemplateRepository.FindBusinessTypeTemplateByIDAsync", timespan.Elapsed, "businessTypeTemplateID={0}", businessTypeTemplateID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in BusinessTypeTemplateRepository.FindBusinessTypeTemplateByIDAsync(businessTypeTemplateID={0})", businessTypeTemplateID);
                throw;
            }

            return businessTypeTemplate;
        }

        public async Task<List<BusinessTypeTemplate>> FindBusinessTypeTemplatesAsync()
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                var result = await db.BusinessTypeTemplates
                    .OrderBy(t => t.BusinessTypeTemplateCode).ToListAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "BusinessTypeTemplateRepository.FindBusinessTypeTemplatesAsync", timespan.Elapsed);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in BusinessTypeTemplateRepository.FindBusinessTypeTemplatesAsync()");
                throw;
            }
        }

        public async Task UpdateAsync(BusinessTypeTemplate businessTypeTemplateToSave)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.Entry(businessTypeTemplateToSave).State = EntityState.Modified;
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "BusinessTypeTemplateRepository.UpdateAsync", timespan.Elapsed, "businessTypeTemplateToSave={0}", businessTypeTemplateToSave);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in BusinessTypeTemplateRepository.UpdateAsync(businessTypeTemplateToSave={0})", businessTypeTemplateToSave);
                throw;
            }
        }

        private void DeleteDocumentationBusinessTypesTemplate(int businessTypeTemplateID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                var docBusinessTypeTemplates = db.DocumentationBusinessTypeTemplates
                    .Where(t => t.BusinessTypeTemplateID == businessTypeTemplateID).ToList();

                foreach (DocumentationBusinessTypeTemplate item in docBusinessTypeTemplates)
                {
                    db.DocumentationBusinessTypeTemplates.Remove(item);
                }
                db.SaveChanges();

                timespan.Stop();
                log.TraceApi("SQL Database", "BusinessTypeTemplateRepository.DeleteDocumentationBusinessTypesTemplate", timespan.Elapsed, "businessTypeTemplateID={0}", businessTypeTemplateID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in BusinessTypeTemplateRepository.DeleteDocumentationBusinessTypesTemplate()");
                throw;
            }
        }
    }
}
