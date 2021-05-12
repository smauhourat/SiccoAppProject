using SiccoApp.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SiccoApp.Persistence
{
    public class DocumentationRepository : IDocumentationRepository
    {
        private SiccoAppContext db = new SiccoAppContext();
        private ILogger log = null;

        public DocumentationRepository(ILogger logger)
        {
            log = logger;
        }

        public async Task CreateAsync(Documentation documentationToAdd)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.Documentations.Add(documentationToAdd);
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "DocumentationRepository.CreateAsync", timespan.Elapsed, "documentationToAdd={0}", documentationToAdd);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in DocumentationRepository.CreateAsync(documentationToAdd={0})", documentationToAdd);
                throw;
            }
        }

        private void DeleteDocumentationBusinessTypes(int documentationID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                var docBusinessType = db.DocumentationBusinessTypes
                    .Where(t => t.DocumentationID == documentationID).ToList();

                foreach (DocumentationBusinessType item in docBusinessType)
                {
                    db.DocumentationBusinessTypes.Remove(item);
                }
                db.SaveChanges();

                timespan.Stop();
                log.TraceApi("SQL Database", "DocumentationRepository.DeleteDocumentationBusinessTypes", timespan.Elapsed, "documentationID={0}", documentationID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in DocumentationRepository.DeleteDocumentationBusinessTypes()");
                throw;
            }
        }

        public async Task DeleteAsync(int documentationID)
        {
            Documentation documentation = null;
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                DeleteDocumentationBusinessTypes(documentationID);

                documentation = await db.Documentations.FindAsync(documentationID);
                db.Documentations.Remove(documentation);
                db.SaveChanges();

                timespan.Stop();
                log.TraceApi("SQL Database", "DocumentationRepository.DeleteAsync", timespan.Elapsed, "documentationID={0}", documentationID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in DocumentationRepository.DeleteAsync(documentationID={0})", documentationID);
                throw;
            }
        }

        public async Task<Documentation> FindDocumentationByIDAsync(int documentationID)
        {
            Documentation documentation = null;
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                documentation = await db.Documentations.FindAsync(documentationID);

                timespan.Stop();
                log.TraceApi("SQL Database", "DocumentationRepository.FindDocumentationByIDAsync", timespan.Elapsed, "documentationID={0}", documentationID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in DocumentationRepository.FindDocumentationByIDAsync(documentationID={0})", documentationID);
                throw;
            }

            return documentation;
        }

        public async Task<List<Documentation>> FindDocumentationsAsync(int customerID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                //var result = await db.Documentations.ToListAsync();
                var result = await db.Documentations 
                    .Where(t => t.CustomerID == customerID)
                    .OrderBy(t => t.DocumentationCode).ToListAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "DocumentationRepository.FindDocumentationsAsync", timespan.Elapsed, "customerID={0}", customerID);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in DocumentationRepository.FindDocumentationsAsync(customerID={0})", customerID);
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

        public async Task UpdateAsync(Documentation documentationToSave)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.Entry(documentationToSave).State = EntityState.Modified;
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "DocumentationRepository.UpdateAsync", timespan.Elapsed, "documentationToSave={0}", documentationToSave);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in DocumentationRepository.UpdateAsync(documentationToSave={0})", documentationToSave);
                throw;
            }
        }

        public List<Documentation> Documentations(int customerID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                //var result = db.Documentations.ToList();
                var result = db.Documentations
                    .Where(t => t.CustomerID == customerID)
                    .OrderByDescending(t => t.DocumentationCode).ToList();

                timespan.Stop();
                log.TraceApi("SQL Database", "DocumentationRepository.Documentations", timespan.Elapsed);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in DocumentationRepository.Documentations()");
                throw;
            }
        }

        public List<Documentation> UnAssignedDocumentations(int businessTypeID, int customerID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                //var result = db.Documentations.ToList();
                var result = db.Documentations
                    .Where(t => t.CustomerID == customerID).ToList();

                var docBusiness = db.DocumentationBusinessTypes
                    .Where(t => t.BusinessTypeID == businessTypeID).ToList();


                foreach (DocumentationBusinessType item in docBusiness)
                {
                    //item.Documentation 
                    if (result.Contains(item.Documentation))
                        result.Remove(item.Documentation);
                }

                timespan.Stop();
                log.TraceApi("SQL Database", "DocumentationRepository.UnAssignedDocumentations", timespan.Elapsed);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in DocumentationRepository.UnAssignedDocumentations()");
                throw;
            }
        }

        public async Task<List<DocumentationResume>> FindDocumentationResumeByCustomerContractorAsync(int customerID, int contractorID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {

                var result = await db.DocumentationsResume
                    .Where(x => x.CustomerID == customerID)
                    .Where(y => y.ContractorID == contractorID)
                    .ToListAsync();

                    //.OrderByDescending(t => t.DocumentationCode).ToList();

                timespan.Stop();
                log.TraceApi("SQL Database", "DocumentationRepository.FindDocumentationResumeByCustomerContractorAsync", timespan.Elapsed);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in DocumentationRepository.FindDocumentationResumeByCustomerContractorAsync()");
                throw;
            }
        }
    }
}
