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
    public class VehicleRepository : IVehicleRepository
    {
        private SiccoAppContext db = new SiccoAppContext();
        private ILogger log = null;

        public VehicleRepository(ILogger logger)
        {
            log = logger;
        }

        private VehicleContract CreateVehicleContract(Vehicle vehicle)
        {
            VehicleContract vehicleContract = new VehicleContract();
            vehicleContract.VehicleID = vehicle.VehicleID;

            Contract contract = db.Contracts.Where(c => c.ContractorID == vehicle.ContractorID).ToList().First();

            vehicleContract.ContractID = contract.ContractID;

            return vehicleContract;
        }

        public async Task CreateAsync(Vehicle vehicleToAdd)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            SiccoAppConfiguration.SuspendExecutionStrategy = true;

            DbContextTransaction tran = db.Database.BeginTransaction();

            try
            {
                db.Vehicles.Add(vehicleToAdd);
                //await db.SaveChangesAsync();
                //Asignacion automatica del vehiculo al primer contrato que encuentre del Contratista (MAL, solo para pruebas)
                //db.VehiclesContracts.Add(CreateVehicleContract(vehicleToAdd));

                await db.SaveChangesAsync();

                tran.Commit();

                timespan.Stop();
                log.TraceApi("SQL Database", "VehicleRepository.CreateAsync", timespan.Elapsed, "vehicleToAdd={0}", vehicleToAdd);
            }
            catch (Exception e)
            {
                tran.Rollback();
                log.Error(e, "Error in VehicleRepository.CreateAsync(employeeToAdd={0})", vehicleToAdd);
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

        public async Task<Vehicle> FindVehicleByIDAsync(int vehicleID)
        {
            Vehicle vehicle = null;
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                vehicle = await db.Vehicles.FindAsync(vehicleID);

                timespan.Stop();
                log.TraceApi("SQL Database", "VehicleRepository.FindVehicleByIDAsync", timespan.Elapsed, "vehicleID={0}", vehicleID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in VehicleRepository.FindVehicleByIDAsync(vehicleID={0})", vehicleID);
                throw;
            }

            return vehicle;
        }

        public async Task<List<Vehicle>> FindVehiclesAsync(int contractorID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                //var result = await db.Contractors.ToListAsync();
                var result = await db.Vehicles
                    .Where(t => t.ContractorID == contractorID)
                    .OrderByDescending(t => t.IdentificationNumber).ToListAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "VehicleRepository.FindEmployeesAsync", timespan.Elapsed, "contractorID={0}", contractorID);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in VehicleRepository.FindEmployeesAsync(customerID={0})", contractorID);
                throw;
            }
        }

        public async Task UpdateAsync(Vehicle vehicleToSave)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.Entry(vehicleToSave).State = EntityState.Modified;
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "VehicleRepository.UpdateAsync", timespan.Elapsed, "vehicleToSave={0}", vehicleToSave);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in VehicleRepository.UpdateAsync(vehicleToSave={0})", vehicleToSave);
                throw;
            }
        }

        private int? GetVehicleContractID(int vehicleID)
        {
            VehicleContract vehicleContract = db.VehiclesContracts.Where(c => c.VehicleID == vehicleID).ToList().FirstOrDefault();
            return vehicleContract?.VehicleContractID;
        }

        public async Task DeleteAsync(int vehicleID)
        {
            Vehicle vehicle = null;
            VehicleContract vehicleContract = null;

            Stopwatch timespan = Stopwatch.StartNew();

            SiccoAppConfiguration.SuspendExecutionStrategy = true;

            DbContextTransaction tran = db.Database.BeginTransaction();

            try
            {
                vehicle = await db.Vehicles.FindAsync(vehicleID);
                db.Vehicles.Remove(vehicle);

                vehicleContract = await db.VehiclesContracts.FindAsync(GetVehicleContractID(vehicleID));
                if (vehicleContract != null)
                    db.VehiclesContracts.Remove(vehicleContract);

                db.SaveChanges();

                tran.Commit();

                timespan.Stop();
                log.TraceApi("SQL Database", "VehicleRepository.DeleteAsync", timespan.Elapsed, "vehicleID={0}", vehicleID);
            }
            catch (Exception e)
            {
                tran.Rollback();
                log.Error(e, "Error in VehicleRepository.DeleteAsync(vehicleID={0})", vehicleID);
                throw;
            }

            SiccoAppConfiguration.SuspendExecutionStrategy = false;
        }

        public async Task DeleteContract(int vehicleID, int contractID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                VehicleContract vehicleContract = db.VehiclesContracts.Where(c => c.VehicleID == vehicleID && c.ContractID == contractID).ToList().First();

                db.VehiclesContracts.Remove(vehicleContract);
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "VehicleRepository.DeleteContract", timespan.Elapsed, "vehicleID={0}, contractID={1}", vehicleID, contractID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in VehicleRepository.DeleteContract(vehicleID={0}, contractID={1})", vehicleID, contractID);
                throw;
            }
        }

        public async Task CreateContractAsync(VehicleContract vehicleContract)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.VehiclesContracts.Add(vehicleContract);

                await db.SaveChangesAsync();

                var contract = db.Contracts.Where(c => c.ContractID == vehicleContract.ContractID).SingleOrDefault<Contract>();

                await GenerateContractorVehicleRequirements(contract.ContractorID, vehicleContract.ContractID, vehicleContract.VehicleID);


                timespan.Stop();
                log.TraceApi("SQL Database", "VehicleRepository.CreateContractAsync", timespan.Elapsed, "vehicleContract={0}", vehicleContract);

            }
            catch (Exception e)
            {

                log.Error(e, "Error in VehicleRepository.CreateContractAsync(vehicleContract={0})", vehicleContract);
                throw;
            }
        }

        public async Task GenerateContractorVehicleRequirements(int contractorID, int contractID, int vehicleID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                await db.Database.ExecuteSqlCommandAsync("dbo.spRequirement_GenerateByContractorVehicleContract @ContractorID, @ContractID, @VehicleID",
                    new SqlParameter("@ContractorID", contractorID),
                    new SqlParameter("@ContractID", contractID),
                    new SqlParameter("@VehicleID", vehicleID));

                timespan.Stop();
                log.TraceApi("SQL Database", "VehicleRepository.GenerateContractorVehicleRequirements", timespan.Elapsed, "contractorID={0}, contractID={1}, vehicleID={2}", contractorID, contractID, vehicleID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in VehicleRepository.GenerateContractorVehicleRequirements(contractorID={0}, contractID={1}, vehicleID={2})", contractorID, contractID, vehicleID);
                throw;
            }
        }

        public async Task<VehicleContract> GetVehicleContract(int vehicleID, int contractID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                VehicleContract vehicleContract = db.VehiclesContracts.Where(c => c.VehicleID == vehicleID && c.ContractID == contractID).ToList().First();

                timespan.Stop();
                log.TraceApi("SQL Database", "VehicleRepository.GetVehicleContract", timespan.Elapsed, "vehicleID={0}, contractID={1}", vehicleID, contractID);

                return await Task.Run(() => { return vehicleContract; });
            }
            catch (Exception e)
            {
                log.Error(e, "Error in VehicleRepository.GetVehicleContract(vehicleID={0}, contractID={1})", vehicleID, contractID);
                throw;
            }
        }

        public List<Vehicle> UnAssignedVehicles(int contractID, int contractorID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                //Todos los vehiculos del Contratista
                var result = db.Vehicles
                    .Where(t => t.ContractorID == contractorID).ToList();

                //Todos los Vehiculos del Contrato
                var vehicleContracts = db.VehiclesContracts
                    .Where(t => t.ContractID == contractID).ToList();


                foreach (VehicleContract item in vehicleContracts)
                {
                    if (result.Contains(item.Vehicle))
                        result.Remove(item.Vehicle);
                }

                timespan.Stop();
                log.TraceApi("SQL Database", "VehicleRepository.UnAssignedVehicles", timespan.Elapsed);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in VehicleRepository.UnAssignedVehicles()");
                throw;
            }
        }
    }
}
