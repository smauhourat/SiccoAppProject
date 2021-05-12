using SiccoApp.Logging;
using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SiccoApp.Persistence
{
    public class RequirementRepository : IRequirementRepository, IDisposable
    {
        private SiccoAppContext db = new SiccoAppContext();
        private ILogger log = null;

        public RequirementRepository(ILogger logger)
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

        //http://stackoverflow.com/questions/8180310/ef4-1-code-first-stored-procedure-with-output-parameter
#warning "Esta todo harcodeada - GenerateByPeriodAsync -"
        public void GenerateByPeriodAsync(int CustomerID, int PeriodID, DateTime DueDate, string Result)
        {
            var outputParam = new SqlParameter();
            outputParam.ParameterName = "@Return";
            outputParam.Direction = System.Data.ParameterDirection.Output;
            outputParam.SqlDbType = System.Data.SqlDbType.Int;

            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.Database.ExecuteSqlCommand("dbo.spRequirement_Generate @CustomerID, @PeriodID, @DueDate, @Return OUT",
                    new SqlParameter("@CustomerID", 1),
                    new SqlParameter("@PeriodID", 201606),
                    new SqlParameter("@DueDate", "06/30/2016"),
                    outputParam);
                //DateTime.UtcNow)

                timespan.Stop();
                log.TraceApi("SQL Database", "CustomerRepository.GenerateByPeriodAsync", timespan.Elapsed);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in CustomerRepository.GenerateByPeriodAsync");
                throw;
            }
        }

        public Task<List<Requirement>> FindRequirementsByFilterAsync(int customerID, int contractorID, int contractID, int periodID, RequirementStatus requirementStatus, int entityTypeID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                var result = db.Requirements
                    .Where(t => t.Contract.Contractor.CustomerID == customerID || customerID == 0)
                    .Where(t => t.Contract.Contractor.ContractorID == contractorID || contractorID == 0)
                    .Where(t => t.ContractID == contractID || contractID == 0)
                    .Where(t => t.PeriodID == periodID || periodID == 0)
                    .Where(t => t.RequirementStatus == requirementStatus || (int)requirementStatus == 0)
                    .Where(t => t.DocumentationBusinessType.Documentation.EntityTypeID == entityTypeID || entityTypeID == 0)
                    .OrderByDescending(t => t.DocumentationBusinessType.Documentation.EntityType.EntityTypeID)
                    .OrderByDescending(t => t.DocumentationBusinessType.Documentation.DocumentationCode).ToListAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "RequirementRepository.FindRequirementsByContractAsync", timespan.Elapsed);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in RequirementRepository.FindRequirementsByContractAsync()");
                throw;
            }
        }

        public async Task<Requirement> FindRequirementByIDAsync(int requirementID)
        {
            Requirement requirement = null;
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                requirement = await db.Requirements.FindAsync(requirementID);

                timespan.Stop();
                log.TraceApi("SQL Database", "RequirementRepository.FindRequirementByIDAsync", timespan.Elapsed, "requirementID={0}", requirementID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in RequirementRepository.FindRequirementByIDAsync(requirementID={0})", requirementID);
                throw;
            }

            return requirement;
        }

        public async Task<List<Presentation>> FindPresentationsAsync(int requirementID)
        {
            Requirement requirement = null;
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                requirement = await db.Requirements.FindAsync(requirementID);
                var result = requirement.Presentations;

                timespan.Stop();
                log.TraceApi("SQL Database", "RequirementRepository.FindRequirementsByContractAsync", timespan.Elapsed);

                return result.ToList();
            }
            catch (Exception e)
            {
                log.Error(e, "Error in RequirementRepository.FindRequirementsByContractAsync()");
                throw;
            }
        }

        public Task<List<Requirement>> FindRequirementsNextToExpireAsync()
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                var result = db.Requirements
                    .Where(t => t.DueDate < DateTime.Now)
                    .ToListAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "RequirementRepository.FindRequirementsNextToExpireAsync", timespan.Elapsed);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in RequirementRepository.FindRequirementsNextToExpireAsync()");
                throw;
            }
        }

        public async Task CreateAsync(Requirement requirementToAdd)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.Requirements.Add(requirementToAdd);
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "RequirementRepository.CreateAsync", timespan.Elapsed, "requirementToAdd={0}", requirementToAdd);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in RequirementRepository.CreateAsync(requirementToAdd={0})", requirementToAdd);
                throw;
            }

            SiccoAppConfiguration.SuspendExecutionStrategy = false;
        }

        public async Task UpdateAsync(Requirement requirementToSave)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.Entry(requirementToSave).State = EntityState.Modified;
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "RequirementRepository.UpdateAsync", timespan.Elapsed, "requirementToSave={0}", requirementToSave);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in RequirementRepository.UpdateAsync(requirementToSave={0})", requirementToSave);
                throw;
            }
        }

        public async Task DeleteAsync(int requirementID)
        {
            Requirement requirement = null;
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                requirement = await db.Requirements.FindAsync(requirementID);
                db.Requirements.Remove(requirement);
                db.SaveChanges();

                timespan.Stop();
                log.TraceApi("SQL Database", "RequirementRepository.DeleteAsync", timespan.Elapsed, "requirementID={0}", requirementID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in RequirementRepository.DeleteAsync(requirementID={0})", requirementID);
                throw;
            }
        }
    }
}
