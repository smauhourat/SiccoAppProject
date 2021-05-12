using SiccoApp.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SiccoApp.Persistence
{
    public class BusinessTypeRepository : IBusinessTypeRepository
    {
        private SiccoAppContext db = new SiccoAppContext();
        private ILogger log = null;

        public BusinessTypeRepository(ILogger logger)
        {
            log = logger;
        }

        public List<BusinessType> BusinessTypes()
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                var result = db.BusinessTypes.ToList();

                timespan.Stop();
                log.TraceApi("SQL Database", "BusinessTypesRepository.BusinessTypes", timespan.Elapsed);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in BusinessTypesRepository.BusinessTypes()");
                throw;
            }
        }

        public List<BusinessType> BusinessTypes(int customerID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                //var result = await db.BusinessTypes.ToListAsync();
                var result = db.BusinessTypes
                    .Where(t => t.CustomerID == customerID)
                    .OrderByDescending(t => t.BusinessTypeCode).ToList();

                timespan.Stop();
                log.TraceApi("SQL Database", "BusinessTypesRepository.BusinessTypes", timespan.Elapsed, "customerID={0}", customerID);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in BusinessTypesRepository.BusinessTypes(customerID={0})", customerID);
                throw;
            }
        }

        public async Task<List<BusinessType>> BusinessTypesAsync()
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                var result = await db.BusinessTypes.ToListAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "BusinessTypesRepository.BusinessTypesAsync", timespan.Elapsed);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in BusinessTypesRepository.BusinessTypesAsync()");
                throw;
            }
        }

        public async Task CreateAsync(BusinessType businessTypeToAdd)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.BusinessTypes.Add(businessTypeToAdd);
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "BusinessTypesRepository.CreateAsync", timespan.Elapsed, "businessTypeTemplateToAdd={0}", businessTypeToAdd);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in BusinessTypesRepository.CreateAsync(businessTypeToAdd={0})", businessTypeToAdd);
                throw;
            }
        }
        public async Task DeleteAsync(int businessTypeID)
        {
            BusinessType businessType = null;
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                DeleteDocumentationBusinessTypes(businessTypeID);

                businessType = await db.BusinessTypes.FindAsync(businessTypeID);
                db.BusinessTypes.Remove(businessType);
                db.SaveChanges();

                timespan.Stop();
                log.TraceApi("SQL Database", "BusinessTypesRepository.DeleteAsync", timespan.Elapsed, "businessTypeID={0}", businessTypeID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in BusinessTypesRepository.DeleteAsync(businessTypeID={0})", businessTypeID);
                throw;
            }
        }
        public async Task<BusinessType> FindBusinessTypeByIDAsync(int businessTypeID)
        {
            BusinessType businessType = null;
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                businessType = await db.BusinessTypes.FindAsync(businessTypeID);

                timespan.Stop();
                log.TraceApi("SQL Database", "BusinessTypesRepository.FindBusinessTypeByIDAsync", timespan.Elapsed, "businessTypeID={0}", businessTypeID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in BusinessTypesRepository.FindBusinessTypeByIDAsync(businessTypeID={0})", businessTypeID);
                throw;
            }

            return businessType;
        }
        public async Task<List<BusinessType>> FindBusinessTypeAsync()
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                var result = await db.BusinessTypes.ToListAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "BusinessTypesRepository.FindBusinessTypeAsync", timespan.Elapsed);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in BusinessTypesRepository.FindBusinessTypeAsync()");
                throw;
            }
        }
        public async Task UpdateAsync(BusinessType businessTypeToSave)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.Entry(businessTypeToSave).State = EntityState.Modified;
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "BusinessTypesRepository.UpdateAsync", timespan.Elapsed, "businessTypeToSave={0}", businessTypeToSave);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in BusinessTypesRepository.UpdateAsync(businessTypeToSave={0})", businessTypeToSave);
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

        public async Task<List<BusinessType>> FindBusinessTypesAsync(int customerID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                //var result = await db.BusinessTypes.ToListAsync();
                var result = await db.BusinessTypes
                    .Where(t => t.CustomerID == customerID)
                    .OrderBy(t => t.BusinessTypeCode).ToListAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "BusinessTypesRepository.FindBusinessTypesAsync", timespan.Elapsed, "customerID={0}", customerID);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in BusinessTypesRepository.FindBusinessTypesAsync(customerID={0})", customerID);
                throw;
            }
        }

        private void DeleteDocumentationBusinessTypes(int businessTypeID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                var docBusinessTypes = db.DocumentationBusinessTypes
                    .Where(t => t.BusinessTypeID == businessTypeID).ToList();

                foreach (DocumentationBusinessType item in docBusinessTypes)
                {
                    db.DocumentationBusinessTypes.Remove(item);
                }
                db.SaveChanges();

                timespan.Stop();
                log.TraceApi("SQL Database", "BusinessTypesRepository.DeleteDocumentationBusinessTypes", timespan.Elapsed, "businessTypeID={0}", businessTypeID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in BusinessTypesRepository.DeleteDocumentationBusinessTypes()");
                throw;
            }
        }
    }
}
