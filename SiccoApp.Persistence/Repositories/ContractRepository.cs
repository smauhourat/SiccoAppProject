using SiccoApp.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;


namespace SiccoApp.Persistence
{
    public class ContractRepository : IContractRepository
    {
        private SiccoAppContext db = new SiccoAppContext();
        private ILogger log = null;

        public ContractRepository(ILogger logger)
        {
            log = logger;
        }

        public async Task<List<EmployeeContract>> FindEmployeeContractsAsync(int contractID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                var result = await db.EmployeesContracts
                    //.Where(t => t.ContractorID == (contractorID == 0 ? t.ContractorID : contractorID))
                    .Where(t => t.ContractID == contractID).ToListAsync();
                //.OrderByDescending(t => t.StartDate).ToListAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "ContractRepository.FindEmployeeContractsAsync", timespan.Elapsed, "contractID={0}", contractID);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in ContractRepository.FindEmployeeContractsAsync(contractID={0})", contractID);
                throw;
            }
        }

        public async Task<List<VehicleContract>> FindVehicleContractsAsync(int contractID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                var result = await db.VehiclesContracts
                    //.Where(t => t.ContractorID == (contractorID == 0 ? t.ContractorID : contractorID))
                    .Where(t => t.ContractID == contractID).ToListAsync();
                //.OrderByDescending(t => t.StartDate).ToListAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "ContractRepository.FindVehicleContractsAsync", timespan.Elapsed, "contractID={0}", contractID);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in ContractRepository.FindVehicleContractsAsync(contractID={0})", contractID);
                throw;
            }
        }

        public async Task<List<Contract>> FindContractsAsync(int contractorID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                var result = await db.Contracts
                    //.Where(t => t.ContractorID == (contractorID == 0 ? t.ContractorID : contractorID))
                    .Where(t => t.ContractorID == contractorID)
                    .OrderByDescending(t => t.StartDate).ToListAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "ContractRepository.FindContractsAsync", timespan.Elapsed, "contractorID={0}", contractorID);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in ContractRepository.FindContractsAsync(contractorID={0})", contractorID);
                throw;
            }
        }

        public async Task<List<Contract>> FindContractsAsync(int customerID, int contractorID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                var result = await db.Contracts
                    .Where(t => t.CustomerID == customerID)
                    .Where(t => t.ContractorID == (contractorID == 0 ? t.ContractorID : contractorID))
                    .OrderByDescending(t => t.StartDate).ToListAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "ContractRepository.FindContractsAsync", timespan.Elapsed, "contractorID={0}", contractorID);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in ContractRepository.FindContractsAsync(contractorID={0})", contractorID);
                throw;
            }
        }

        public async Task<Contract> FindContractByIDAsync(int contractID)
        {
            Contract contract = null;
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                contract = await db.Contracts.FindAsync(contractID);

                timespan.Stop();
                log.TraceApi("SQL Database", "ContractRepository.FindContractByIDAsync", timespan.Elapsed, "contractID={0}", contractID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in ContractRepository.FindContractByIDAsync(contractID={0})", contractID);
                throw;
            }

            return contract;
        }

        public async Task UpdateAsync(Contract contractToSave)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.Entry(contractToSave).State = EntityState.Modified;
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "ContractRepository.UpdateAsync", timespan.Elapsed, "contractToSave={0}", contractToSave);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in ContractRepository.UpdateAsync(vehicleToSave={0})", contractToSave);
                throw;
            }
        }

        public async Task CreateAsync(Contract contractToAdd)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            SiccoAppConfiguration.SuspendExecutionStrategy = true;

            DbContextTransaction tran = db.Database.BeginTransaction();

            try
            {
                db.Contracts.Add(contractToAdd);
                //await db.SaveChangesAsync();

                //db.VehiclesContracts.Add(CreateVehicleContract(contractToAdd));

                await db.SaveChangesAsync();

                tran.Commit();

                timespan.Stop();
                log.TraceApi("SQL Database", "ContractRepository.CreateAsync", timespan.Elapsed, "contractToAdd={0}", contractToAdd);
            }
            catch (Exception e)
            {
                tran.Rollback();
                log.Error(e, "Error in ContractRepository.CreateAsync(contractToAdd={0})", contractToAdd);
                throw;
            }

            SiccoAppConfiguration.SuspendExecutionStrategy = false;
        }

        public async Task DeleteAsync(int contractID)
        {
            Contract contract = null;
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                contract = await db.Contracts.FindAsync(contractID);
                db.Contracts.Remove(contract);
                db.SaveChanges();

                timespan.Stop();
                log.TraceApi("SQL Database", "contractsRepository.DeleteAsync", timespan.Elapsed, "contractID={0}", contractID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in contractsRepository.DeleteAsync(contractID={0})", contractID);
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
