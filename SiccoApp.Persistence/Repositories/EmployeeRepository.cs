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
    public class EmployeeRepository : IEmployeeRepository
    {
        private SiccoAppContext db = new SiccoAppContext();
        private ILogger log = null;

        public EmployeeRepository(ILogger logger)
        {
            log = logger;
        }

        //private EmployeeContract CreateEmployeeContract(Employee employee)
        //{
        //    EmployeeContract employeeContract = new EmployeeContract();
        //    employeeContract.EmployeeID = employee.EmployeeID;

        //    Contract contract = db.Contracts.Where(c => c.ContractorID == employee.ContractorID).ToList().First();

        //    employeeContract.ContractID = contract.ContractID;

        //    return employeeContract;
        //}

        public async Task CreateAsync(Employee employeeToAdd)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            SiccoAppConfiguration.SuspendExecutionStrategy = true;

            DbContextTransaction tran = db.Database.BeginTransaction();

            try
            {
                db.Employees.Add(employeeToAdd);
                //await db.SaveChangesAsync();
                //Asignacion automatica del empleado al primer contrato que encuentre del Contratista (MAL, solo para pruebas)
                //db.EmployeesContracts.Add(CreateEmployeeContract(employeeToAdd));

                await db.SaveChangesAsync();

                tran.Commit();

                timespan.Stop();
                log.TraceApi("SQL Database", "EmployeeRepository.CreateAsync", timespan.Elapsed, "employeeToAdd={0}", employeeToAdd);
            }
            catch (Exception e)
            {
                tran.Rollback();
                log.Error(e, "Error in EmployeeRepository.CreateAsync(employeeToAdd={0})", employeeToAdd);
                throw;
            }

            SiccoAppConfiguration.SuspendExecutionStrategy = false;
        }

        public async Task<List<Employee>> FindEmployeesAsync(int contractorID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                //var result = await db.Contractors.ToListAsync();
                var result = await db.Employees
                    .Where(t => t.ContractorID == contractorID)
                    .OrderByDescending(t => t.LastName).ToListAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "EmployeeRepository.FindEmployeesAsync", timespan.Elapsed, "customerID={0}", contractorID);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in EmployeeRepository.FindEmployeesAsync(customerID={0})", contractorID);
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

        public async Task<Employee> FindEmployeeByIDAsync(int employeeID)
        {
            Employee employee = null;
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                employee = await db.Employees.FindAsync(employeeID);

                timespan.Stop();
                log.TraceApi("SQL Database", "EmployeeRepository.FindEmployeeByIDAsync", timespan.Elapsed, "employeeID={0}", employeeID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in EmployeeRepository.FindEmployeeByIDAsync(employeeID={0})", employeeID);
                throw;
            }

            return employee;
        }

        public async Task UpdateAsync(Employee employeeToSave)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.Entry(employeeToSave).State = EntityState.Modified;
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "EmployeeRepository.UpdateAsync", timespan.Elapsed, "employeeToSave={0}", employeeToSave);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in EmployeeRepository.UpdateAsync(employeeToSave={0})", employeeToSave);
                throw;
            }
        }

        private int? GetEmployeeContractID(int employeeID)
        {
            EmployeeContract employeeContract = db.EmployeesContracts.Where(c => c.EmployeeID == employeeID).ToList().FirstOrDefault();
            return employeeContract?.EmployeeContractID;
        }

        public async Task DeleteAsync(int employeeID)
        {
            Employee employee = null;
            EmployeeContract employeeContract = null;

            Stopwatch timespan = Stopwatch.StartNew();

            SiccoAppConfiguration.SuspendExecutionStrategy = true;

            DbContextTransaction tran = db.Database.BeginTransaction();

            try
            {
                employee = await db.Employees.FindAsync(employeeID);
                db.Employees.Remove(employee);

                employeeContract = await db.EmployeesContracts.FindAsync(GetEmployeeContractID(employeeID));
                if (employeeContract != null)
                    db.EmployeesContracts.Remove(employeeContract);

                db.SaveChanges();

                tran.Commit();

                timespan.Stop();
                log.TraceApi("SQL Database", "EmployeeRepository.DeleteAsync", timespan.Elapsed, "employeeID={0}", employeeID);
            }
            catch (Exception e)
            {
                tran.Rollback();
                log.Error(e, "Error in EmployeeRepository.DeleteAsync(employeeID={0})", employeeID);
                throw;
            }

            SiccoAppConfiguration.SuspendExecutionStrategy = false;
        }

        public async Task DeleteContract(int employeeID, int contractID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                EmployeeContract employeeContract = db.EmployeesContracts.Where(c => c.EmployeeID == employeeID && c.ContractID == contractID).ToList().First();

                db.EmployeesContracts.Remove(employeeContract);
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "EmployeeRepository.DeleteContract", timespan.Elapsed, "employeeID={0}, contractID={1}", employeeID, contractID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in EmployeeRepository.DeleteContract(employeeID={0}, contractID={1})", employeeID, contractID);
                throw;
            }
        }

        //public async Task CreateContractAsync(EmployeeContract contract)
        //{
        //    Stopwatch timespan = Stopwatch.StartNew();

        //    try
        //    {
        //        db.EmployeesContracts.Add(contract);

        //        await db.SaveChangesAsync();

        //        timespan.Stop();
        //        log.TraceApi("SQL Database", "EmployeeRepository.CreateContractAsync", timespan.Elapsed, "contract={0}", contract);

        //    }
        //    catch (Exception e)
        //    {

        //        log.Error(e, "Error in EmployeeRepository.CreateContractAsync(contract={0})", contract);
        //        throw;
        //    }
        //}

        public async Task CreateContractAsync(EmployeeContract employeeContract)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            SiccoAppConfiguration.SuspendExecutionStrategy = true;

            DbContextTransaction tran = db.Database.BeginTransaction();

            try
            {
                db.EmployeesContracts.Add(employeeContract);

                await db.SaveChangesAsync();

                tran.Commit();

                var contract = db.Contracts.Where(c => c.ContractID == employeeContract.ContractID).SingleOrDefault<Contract>();

                await GenerateContractorEmployeeRequirements(contract.ContractorID, employeeContract.ContractID, employeeContract.EmployeeID);

                timespan.Stop();
                log.TraceApi("SQL Database", "EmployeeRepository.CreateContractAsync", timespan.Elapsed, "employeeContract={0}", employeeContract);

            }
            catch (Exception e)
            {
                tran.Rollback();
                log.Error(e, "Error in EmployeeRepository.CreateContractAsync(employeeContract={0})", employeeContract);
                throw;
            }
        }

        public async Task GenerateContractorEmployeeRequirements(int contractorID, int contractID, int employeeID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                await db.Database.ExecuteSqlCommandAsync("dbo.spRequirement_GenerateByContractorEmployeeContract @ContractorID, @ContractID, @EmployeeID", 
                    new SqlParameter("@ContractorID", contractorID),
                    new SqlParameter("@ContractID", contractID),
                    new SqlParameter("@EmployeeID", employeeID));

                timespan.Stop();
                log.TraceApi("SQL Database", "EmployeeRepository.GenerateContractorEmployeeRequirements", timespan.Elapsed, "contractorID={0}, contractID={1}, employeeID={2}", contractorID, contractID, employeeID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in EmployeeRepository.GenerateContractorEmployeeRequirements(contractorID={0}, contractID={1}, employeeID={2})", contractorID, contractID, employeeID);
                throw;
            }
        }

        public async Task<EmployeeContract> GetEmployeeContract(int employeeID, int contractID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                EmployeeContract employeeContract = db.EmployeesContracts.Where(c => c.EmployeeID == employeeID && c.ContractID == contractID).ToList().First();

                timespan.Stop();
                log.TraceApi("SQL Database", "EmployeeRepository.GetEmployeeContract", timespan.Elapsed, "employeeID={0}, contractID={1}", employeeID, contractID);

                return await Task.Run(() => { return employeeContract; });
            }
            catch (Exception e)
            {
                log.Error(e, "Error in EmployeeRepository.GetEmployeeContract(employeeID={0}, contractID={1})", employeeID, contractID);
                throw;
            }
        }

        public List<Employee> UnAssignedEmployees(int contractID, int contractorID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                //Todos los emopleados del Contratista
                var result = db.Employees
                    .Where(t => t.ContractorID == contractorID).ToList();

                //Todos los Empleados del Contrato
                var employeeContracts = db.EmployeesContracts
                    .Where(t => t.ContractID == contractID).ToList();


                foreach (EmployeeContract item in employeeContracts)
                {
                    //item.Documentation 
                    if (result.Contains(item.Employee))
                        result.Remove(item.Employee);
                }

                timespan.Stop();
                log.TraceApi("SQL Database", "EmployeeRepository.UnAssignedEmployees", timespan.Elapsed);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in EmployeeRepository.UnAssignedEmployees()");
                throw;
            }
        }
    }
}
