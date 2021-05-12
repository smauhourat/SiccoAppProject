using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SiccoApp.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SiccoApp.Persistence
{
    public class CustomerAuditorRespository : ICustomerAuditorRespository
    {
        private SiccoAppContext db = new SiccoAppContext();
        private UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
        private ILogger log = null;

        public CustomerAuditorRespository(ILogger logger)
        {
            log = logger;
        }

        public void SetRole(string UserId)
        {
            var idManager = new IdentityManager();
            idManager.AddUserToRole(UserId, "CustomerAuditorRole");
            idManager = null;
        }

#warning Ver como hacer para trabajar con AspNetIdentity de forma asincronica, porq no funca.
        public void Create(CustomerAuditor customerAuditorToAdd)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.CustomerAuditors.Add(customerAuditorToAdd);
                db.SaveChanges();

                //Seteamos el Rol 
                SetRole(customerAuditorToAdd.UserId);

                timespan.Stop();
                log.TraceApi("SQL Database", "CustomerAuditorRespository.Create", timespan.Elapsed, "customerAuditorToAdd={0}", customerAuditorToAdd);
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    //Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:", eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    log.Error(e, "Entity of type \"{0}\" in state \"{1}\" has the following validation errors:", eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        //Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage);
                        log.Error(e, "- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in CustomerAuditorRespository.CreateAsync(customerAuditorToAdd={0})", customerAuditorToAdd);
                throw;
            }
        }

        public async Task CreateAsync(CustomerAuditor customerAuditorToAdd)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.CustomerAuditors.Add(customerAuditorToAdd);
                await db.SaveChangesAsync();

                //Seteamos el Rol 
                SetRole(customerAuditorToAdd.UserId);

                timespan.Stop();
                log.TraceApi("SQL Database", "CustomerAuditorRespository.CreateAsync", timespan.Elapsed, "customerAuditorToAdd={0}", customerAuditorToAdd);
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    //Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:", eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    log.Error(e, "Entity of type \"{0}\" in state \"{1}\" has the following validation errors:", eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        //Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage);
                        log.Error(e, "- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in CustomerAuditorRespository.CreateAsync(customerAuditorToAdd={0})", customerAuditorToAdd);
                throw;
            }
        }

        public async Task<List<ApplicationUser>> FindCustomerAuditorsAsync()
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                //var result = await userManager.Users.Where(x => x.Roles.Select(y => y.RoleId).Contains("3A9E89EC-4D3C-44CF-A0F6-87FDFA8C20A6")).ToListAsync();

                var result = await userManager.Users
                    .Where(x => x.Roles.Select(y => y.RoleId).Contains("3A9E89EC-4D3C-44CF-A0F6-87FDFA8C20A6")) //CustomerAuditorRole
                    .Where(z => ! z.Roles.Select(w => w.RoleId).Contains("E59357F8-9D02-43C0-93B0-657249A1E26D")) //AdminRole
                    .Where(z => !z.Roles.Select(w => w.RoleId).Contains("881EFE1D-6907-426D-BEA3-CF9905BD0806")) //ContractorRole
                    .Where(z => !z.Roles.Select(w => w.RoleId).Contains("A721FB75-8CAF-4C21-98B9-D5696484AE65")) //CustomerAdminRole
                    .Where(z => !z.Roles.Select(w => w.RoleId).Contains("6F848BD4-77DF-43FE-BE46-6A502090DCEF")) //CustomerAdminUserRole
                    .Where(z => !z.Roles.Select(w => w.RoleId).Contains("460FC984-F031-459F-AC8D-180449ADD457")) //CustomerRole
                    .ToListAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "CustomerAuditorRespository.FindCustomerAuditorsAsync", timespan.Elapsed);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in CustomerAuditorRespository.FindCustomerAuditorsAsync()");
                throw;
            }

        }

        public async Task<List<CustomerAuditor>> FindCustomerAuditorsByCustomerAsync(int? customerID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                var result = await db.CustomerAuditors
                    .Where(t => t.CustomerID == (int?)customerID).ToListAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "CustomerAuditorRespository.FindCustomerAuditorsByCustomerAsync", timespan.Elapsed);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in CustomerAuditorRespository.FindCustomerAuditorsByCustomerAsync()");
                throw;
            }
        }

        public async Task<List<CustomerAuditor>> FindCustomerAuditorsByUserIDAsync(string userID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                var result = await db.CustomerAuditors
                    .Where(t => t.UserId == userID).ToListAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "CustomerAuditorRespository.FindCustomerAuditorsByUserIDAsync", timespan.Elapsed);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in CustomerAuditorRespository.FindCustomerAuditorsByUserIDAsync()");
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

        private void DeleteUser(string userId)
        {
            var idManager = new IdentityManager();
            idManager.DeleteUser(userId);
            idManager = null;
        }

        public async Task Delete(int customerAuditorID)
        {
            CustomerAuditor customerAuditor = null;
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {                
                customerAuditor = await db.CustomerAuditors.FindAsync(customerAuditorID);
                var userId = customerAuditor.UserId;
                db.CustomerAuditors.Remove(customerAuditor);
                db.SaveChanges();

                DeleteUser(userId);

                timespan.Stop();
                log.TraceApi("SQL Database", "CustomerAuditorRespository.Delete", timespan.Elapsed, "customerAuditorID={0}", customerAuditorID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in CustomerAuditorRespository.Delete(customerAuditorID={0})", customerAuditorID);
                throw;
            }
        }

        public async Task<CustomerAuditor> FindByIdAsync(int customerAuditorID)
        {
            CustomerAuditor customerAuditor = null;
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                customerAuditor = await db.CustomerAuditors.FindAsync(customerAuditorID);

                timespan.Stop();
                log.TraceApi("SQL Database", "CustomerAuditorRespository.FindByIdAsync", timespan.Elapsed, "customerAuditorID={0}", customerAuditorID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in CustomerAuditorRespository.FindByIdAsync(customerAuditorID={0})", customerAuditorID);
                throw;
            }

            return customerAuditor;
        }

        public void Update(CustomerAuditor customerAuditor)
        {
            //throw new NotImplementedException();

            var idManager = new IdentityManager();
            var result = idManager.UpdateUser(customerAuditor.User);

        }

        public async Task DeleteAsync(int customerAuditorID)
        {
            CustomerAuditor customerAuditor = null;
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                customerAuditor = await db.CustomerAuditors.FindAsync(customerAuditorID);
                db.CustomerAuditors.Remove(customerAuditor);
                db.SaveChanges();

                timespan.Stop();
                log.TraceApi("SQL Database", "CustomerAuditorRespository.DeleteAsync", timespan.Elapsed, "customerAuditorID={0}", customerAuditorID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in CustomerAuditorRespository.DeleteAsync(customerAuditorID={0})", customerAuditorID);
                throw;
            }
        }

        public async Task DeleteByUserIdAsync(string userId)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                var customerAuditors = await db.CustomerAuditors.Where(x => x.UserId == userId).ToListAsync();

                foreach (var item in customerAuditors)
                {
                    db.CustomerAuditors.Remove(item);
                    db.SaveChanges();
                }

                timespan.Stop();
                log.TraceApi("SQL Database", "CustomerAuditorRespository.DeleteByUserIdAsync", timespan.Elapsed, "userId={0}", userId);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in CustomerAuditorRespository.DeleteByUserIdAsync(userId={0})", userId);
                throw;
            }
        }

    }
}
