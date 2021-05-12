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
    public class CustomerRepository : ICustomerRepository, IDisposable
    {
        private SiccoAppContext db = new SiccoAppContext();
        private ILogger log = null;

        public CustomerRepository(ILogger logger)
        {
            log = logger;
        }

        public async Task<List<Customer>> FindCustomersAsync()
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                var result = await db.Customers.ToListAsync();
                //var result = await db.FixItTasks
                //    .Where(t => t.Owner == userName)
                //    .Where(t => t.IsDone == false)
                //    .OrderByDescending(t => t.FixItTaskId).ToListAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "CustomerRepository.FindCustomersAsync", timespan.Elapsed);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in CustomerRepository.FindCustomersAsync()");
                throw;
            }
        }

        public List<Customer> Customers()
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                var result = db.Customers.ToList();
                //var result = await db.FixItTasks
                //    .Where(t => t.Owner == userName)
                //    .Where(t => t.IsDone == false)
                //    .OrderByDescending(t => t.FixItTaskId).ToListAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "CustomerRepository.Customers", timespan.Elapsed);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in CustomerRepository.Customers()");
                throw;
            }
        }

        public async Task CreateAsync(Customer customerToAdd)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.Customers.Add(customerToAdd);
                await db.SaveChangesAsync();

                await GenerateDocumentationMatrix(customerToAdd.CustomerID, true);

                timespan.Stop();
                log.TraceApi("SQL Database", "CustomerRepository.CreateAsync", timespan.Elapsed, "customerToAdd={0}", customerToAdd);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in CustomerRepository.CreateAsync(customerToAdd={0})", customerToAdd);
                throw;
            }
        }

        public async Task GenerateDocumentationMatrix(int customerID, bool forceDelete)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                await db.Database.ExecuteSqlCommandAsync("dbo.spDocumentationMatrix_Generate @CustomerID, @ForceDelete",
                    new SqlParameter("@CustomerID", customerID),
                    new SqlParameter("@ForceDelete", forceDelete));


                timespan.Stop();
                log.TraceApi("SQL Database", "CustomerRepository.GenerateDocumentationMatrix", timespan.Elapsed);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in CustomerRepository.GenerateDocumentationMatrix");
                throw;
            }
        }
        
        public async Task DeleteDocumentationMatrix(int customerID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                await db.Database.ExecuteSqlCommandAsync("dbo.spDocumentationMatrix_Delete @CustomerID",
                    new SqlParameter("@CustomerID", customerID));

                //OJOOO
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "CustomerRepository.DeleteDocumentationMatrix", timespan.Elapsed);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in CustomerRepository.DeleteDocumentationMatrix");
                throw;
            }
        }

        //public async Task GenerateDocumentationMatrix(int customerID)
        //{
        //    var outputParam = new SqlParameter();
        //    outputParam.ParameterName = "@Return";
        //    outputParam.Direction = System.Data.ParameterDirection.Output;
        //    //outputParam.SqlDbType = System.Data.SqlDbType.Int;
        //    outputParam.SqlDbType = System.Data.SqlDbType.VarChar;
        //    outputParam.Size = 150;

        //    Stopwatch timespan = Stopwatch.StartNew();

        //    try
        //    {
        //        await db.Database.ExecuteSqlCommandAsync("dbo.spDocumentationMatrix_Generate @CustomerID, @Return OUT",
        //            new SqlParameter("@CustomerID", customerID),
        //            outputParam);

        //        //if (outputParam.Value.ToString() != "") return Task<outputParam.Value>;

        //        timespan.Stop();
        //        log.TraceApi("SQL Database", "CustomerRepository.GenerateDocumentationMatrix", timespan.Elapsed);
        //    }
        //    catch (Exception e)
        //    {
        //        log.Error(e, "Error in CustomerRepository.GenerateDocumentationMatrix");
        //        throw;
        //    }
        //}

        //public void Create(Customer customerToAdd)
        //{
        //    Stopwatch timespan = Stopwatch.StartNew();

        //    try
        //    {
        //        db.Customers.Add(customerToAdd);
        //        db.SaveChanges();

        //        timespan.Stop();
        //        log.TraceApi("SQL Database", "CustomerRepository.CreateAsync", timespan.Elapsed, "customerToAdd={0}", customerToAdd);
        //    }
        //    catch (Exception e)
        //    {
        //        log.Error(e, "Error in CustomerRepository.CreateAsync(customerToAdd={0})", customerToAdd);
        //        throw;
        //    }
        //}

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

        public async Task<Customer> FindCustomerByIDAsync(int customerID)
        {
            Customer customer = null;
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                customer = await db.Customers.FindAsync(customerID);

                timespan.Stop();
                log.TraceApi("SQL Database", "CustomerRepository.FindCustomerByIDAsync", timespan.Elapsed, "customerID={0}", customerID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in CustomerRepository.FindCustomerByIDAsync(customerID={0})", customerID);
                throw;
            }

            return customer;
        }

        public async Task DeleteAsync(int customerID)
        {
            Customer customer = null;
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                await DeleteDocumentationMatrix(customerID);

                customer = await db.Customers.FindAsync(customerID);
                db.Customers.Remove(customer);
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "CustomerRepository.DeleteAsync", timespan.Elapsed, "customerID={0}", customerID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in CustomerRepository.DeleteAsync(customerID={0})", customerID);
                throw;
            }
        }

        public async Task UpdateAsync(Customer customerToSave)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.Entry(customerToSave).State = EntityState.Modified;
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "CustomerRepository.UpdateAsync", timespan.Elapsed, "customerToSave={0}", customerToSave);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in CustomerRepository.UpdateAsync(customerToSave={0})", customerToSave);
                throw;
            }
        }

        public List<Customer> UnAssignedCustomers(string userID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                var result = db.Customers.ToList();

                var auditors = db.CustomerAuditors
                    .Where(t => t.UserId == userID).ToList();

                foreach (CustomerAuditor item in auditors)
                {
                    if (result.Contains(item.Customer))
                        result.Remove(item.Customer);
                }

                timespan.Stop();
                log.TraceApi("SQL Database", "CustomerRepository.UnAssignedCustomers", timespan.Elapsed);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in CustomerRepository.UnAssignedCustomers()");
                throw;
            }
        }
    }
}
