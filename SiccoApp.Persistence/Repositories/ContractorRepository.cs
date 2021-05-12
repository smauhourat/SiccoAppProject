using SiccoApp.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SiccoApp.Persistence
{
    public class ContractorRepository : IContractorRepository
    {
        private SiccoAppContext db = new SiccoAppContext();
        private ILogger log = null;

        public ContractorRepository(ILogger logger)
        {
            log = logger;
        }

        public async Task<List<Contractor>> FindContractorsAsync()
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                var result = await db.Contractors.ToListAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "ContractorRepository.FindContractorsAsync", timespan.Elapsed);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in ContractorRepository.FindContractorsAsync()");
                throw;
            }
        }

        public List<Contractor> FindContractorsByCustomerID(int customerID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                var result = db.Contractors
                    .Where(t => t.CustomerID == customerID)
                    .OrderByDescending(t => t.CompanyName).ToList();

                timespan.Stop();
                log.TraceApi("SQL Database", "ContractorRepository.FindContractorsByCustomerID", timespan.Elapsed, "customerID={0}", customerID);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in ContractorRepository.FindContractorsByCustomerID(customerID={0})", customerID);
                throw;
            }
        }

        public async Task<List<Contractor>> FindContractorsByCustomerIDAsync(int customerID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                var result = await db.Contractors
                    .Where(t => t.CustomerID == customerID)
                    .OrderByDescending(t => t.CompanyName).ToListAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "ContractorRepository.FindContractorsByCustomerIDAsync", timespan.Elapsed, "customerID={0}", customerID);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in ContractorRepository.FindContractorsByCustomerIDAsync(customerID={0})", customerID);
                throw;
            }
        }

        public async Task<List<Contractor>> FindContractorsAsync(int customerID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                //var result = await db.Contractors.ToListAsync();
                var result = await db.Contractors
                    .Where(t => t.CustomerID == customerID)
                    .OrderByDescending(t => t.CompanyName).ToListAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "ContractorRepository.FindContractorsAsync", timespan.Elapsed, "customerID={0}", customerID);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in ContractorRepository.FindContractorsAsync(customerID={0})", customerID);
                throw;
            }
        }

        private Contract CreateContract(Contractor contractor)
        {
            Contract contract = new Contract();
            contract.ContractorID = contractor.ContractorID;
            contract.CustomerID = contractor.CustomerID;
            contract.Description = "Automatic Generated";
            contract.ContractStatusID = ContractStatus.Active;
            //no anda
            //contract.ContractCode = "CON - " + contractor.CustomerID + " - " + DateTime.UtcNow.ToString("yyyyMMdd");
            contract.StartDate = DateTime.UtcNow;

            return contract;
        }

        //https://msdn.microsoft.com/en-us/data/dn307226
        public async Task CreateAsync(Contractor contractorToAdd)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            //Se suspende momentaneamente la estrategia de Azure (reintentos), porque la misma no 
            //soporta manejo de transacciones por parte del usuario
            SiccoAppConfiguration.SuspendExecutionStrategy = true;

            DbContextTransaction tran = db.Database.BeginTransaction();

            try
            {
                db.Contractors.Add(contractorToAdd);
                //await db.SaveChangesAsync();

                var contract = CreateContract(contractorToAdd);
                db.Contracts.Add(contract);

                await db.SaveChangesAsync();

                tran.Commit();

                await GenerateContractorRequirements(contractorToAdd.ContractorID, contract.ContractID);

                timespan.Stop();
                log.TraceApi("SQL Database", "ContractorRepository.CreateAsync", timespan.Elapsed, "customerToAdd={0}", contractorToAdd);
            }
            catch (Exception e)
            {
                tran.Rollback();
                log.Error(e, "Error in ContractorRepository.CreateAsync(contractorToAdd={0})", contractorToAdd);
                throw;
            }

            SiccoAppConfiguration.SuspendExecutionStrategy = false;

        }

        private int? GetContractID(int contractorID)
        {
            Contract contract = db.Contracts.Where(c => c.ContractorID == contractorID).ToList().FirstOrDefault();
            return contract?.ContractID;
        }

        public async Task DeleteAsync(int contractorID)
        {
            Contractor contractor = null;
            Contract contract = null;
            Stopwatch timespan = Stopwatch.StartNew();

            SiccoAppConfiguration.SuspendExecutionStrategy = true;

            DbContextTransaction tran = db.Database.BeginTransaction();

            try
            {
                contractor = await db.Contractors.FindAsync(contractorID);

                contract = await db.Contracts.FindAsync(GetContractID(contractorID));
                if (contract != null)
                    db.Contracts.Remove(contract);

                db.Contractors.Remove(contractor);

                //contract = await db.Contracts.FindAsync(GetContractID(contractorID));
                //if (contract != null)
                //    db.Contracts.Remove(contract);

                db.SaveChanges();

                tran.Commit();

                timespan.Stop();
                log.TraceApi("SQL Database", "ContractorRepository.DeleteAsync", timespan.Elapsed, "contractorID={0}", contractorID);
            }
            catch (Exception e)
            {
                tran.Rollback();
                log.Error(e, "Error in ContractorRepository.DeleteAsync(contractorID={0})", contractorID);
                throw;
            }

            SiccoAppConfiguration.SuspendExecutionStrategy = false;
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

        public async Task<Contractor> FindContractorsByIDAsync(int contractorID)
        {
            Contractor contractor = null;
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                contractor = await db.Contractors.FindAsync(contractorID);

                timespan.Stop();
                log.TraceApi("SQL Database", "ContractorRepository.FindContractorsByIDAsync", timespan.Elapsed, "contractorID={0}", contractorID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in ContractorRepository.FindContractorsByIDAsync(contractorID={0})", contractorID);
                throw;
            }

            return contractor;
        }

        public async Task<Contractor> FindContractorsByIDAsync(int contractorID, int customerID)
        {
            Contractor contractor = null;
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                contractor = await db.Contractors.FindAsync(contractorID);

                timespan.Stop();
                log.TraceApi("SQL Database", "ContractorRepository.FindContractorsByIDAsync", timespan.Elapsed, "contractorID={0}", contractorID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in ContractorRepository.FindContractorsByIDAsync(contractorID={0})", contractorID);
                throw;
            }

            if (contractor.CustomerID != customerID)
            {
                log.Error("Error in ContractorRepository.FindContractorsByIDAsync(contractorID={0})", contractorID);
                throw new Exception("Error in ContractorRepository.FindContractorsByIDAsync(contractorID="+ contractorID.ToString() + ", customerID="+ customerID.ToString() + ")");
            }

            return contractor;
        }

        public async Task UpdateAsync(Contractor contractorToSave)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.Entry(contractorToSave).State = EntityState.Modified;
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "ContractorRepository.UpdateAsync", timespan.Elapsed, "contractorToSave={0}", contractorToSave);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in ContractorRepository.UpdateAsync(contractorToSave={0})", contractorToSave);
                throw;
            }
        }

        public async Task GenerateContractorRequirements(int contractorID, int contractID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                await db.Database.ExecuteSqlCommandAsync("dbo.spRequirement_GenerateByContractorContract @ContractorID, @ContractID",
                    new SqlParameter("@ContractorID", contractorID),
                    new SqlParameter("@ContractID", contractID));

                timespan.Stop();
                log.TraceApi("SQL Database", "ContractorRepository.GenerateContractorRequirements", timespan.Elapsed, "contractorID={0}, contractID={1}", contractorID, contractID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in ContractorRepository.GenerateContractorRequirements(contractorID={0}, contractID={1})", contractorID, contractID);
                throw;
            }
        }

        public async Task GenerateContractorRequirementsAll(int contractorID, int contractID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                await db.Database.ExecuteSqlCommandAsync("dbo.spRequirement_GenerateByContractorContractAll @ContractorID, @ContractID",
                    new SqlParameter("@ContractorID", contractorID),
                    new SqlParameter("@ContractID", contractID));

                timespan.Stop();
                log.TraceApi("SQL Database", "ContractorRepository.GenerateContractorRequirements", timespan.Elapsed, "contractorID={0}, contractID={1}", contractorID, contractID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in ContractorRepository.GenerateContractorRequirements(contractorID={0}, contractID={1})", contractorID, contractID);
                throw;
            }
        }
    }
}
