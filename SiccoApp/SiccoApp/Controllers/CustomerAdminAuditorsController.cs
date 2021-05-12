using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SiccoApp.Persistence;
using System.Threading.Tasks;
using Resources;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SiccoApp.Models;
using SiccoApp.Services;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.AspNet.Identity.Owin;

namespace SiccoApp.Controllers
{
    public class CustomerAdminAuditorsController : BaseController
    {
        private ICustomerAuditorRespository customerAuditorRepository = null;
        private ICustomerRepository customerRepository = null;
        private IWorkflowMessageService workflowMessageService = null;
        private IIdentityMessageService emailService = null;

        //ojoooo no me gusta
        private UserManager<ApplicationUser> userManager = null;

        public CustomerAdminAuditorsController(ICustomerAuditorRespository customerAuditorRepo, 
            ICustomerRepository customerRepo, 
            IWorkflowMessageService workflowMessageServ,
            IIdentityMessageService emailServ)
        {
            customerAuditorRepository = customerAuditorRepo;
            customerRepository = customerRepo;
            workflowMessageService = workflowMessageServ;
            emailService = emailServ;

            userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            userManager.UserValidator = new UserValidator<ApplicationUser>(userManager)
            {
                AllowOnlyAlphanumericUserNames = true,
                RequireUniqueEmail = true
            };

            var provider = new DpapiDataProtectionProvider("SampleAppName");
            userManager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(provider.Create("SampleTokenName"));
            userManager.EmailService = emailService;
        }

        //[Authorize(Roles = "AdminRole,CustomerRole,CustomerAdminRole, CustomerAuditorRole")]
        [AuthorizeRoles(RoleNames.AdminRole, RoleNames.CustomerRole, RoleNames.CustomerAdminRole, RoleNames.CustomerAuditorRole)]
        public async Task<ActionResult> Users()
        {
            //var customerAuditors = await customerAuditorRepository.FindCustomerAuditorsByCustomerAsync(null);
            var customerAuditors = await customerAuditorRepository.FindCustomerAuditorsAsync();

            var customerAdminAuditors = customerAuditors.Select(x => new CustomerAuditor() { UserId = x.Id, User = (AdminUser)x }).ToList();

            var model = new CustomerAuditorsListViewModel(base.CurrentCustomerID, customerAdminAuditors);

            //return View(customerAuditors.ToList());
            return View(model);
        }

        //[Authorize(Roles = "AdminRole,CustomerRole,CustomerAdminRole, CustomerAuditorRole")]
        [AuthorizeRoles(RoleNames.AdminRole, RoleNames.CustomerRole, RoleNames.CustomerAdminRole, RoleNames.CustomerAuditorRole)]
        public ActionResult UsersCreate(int customerID)
        {
            RegisterCustomerAuditorUserViewModel model = new RegisterCustomerAuditorUserViewModel();
            model.CustomerID = customerID;
            return View(model);
        }

        [HttpPost]
        //[Authorize(Roles = "AdminRole,CustomerRole,CustomerAdminRole, CustomerAuditorRole")]
        [AuthorizeRoles(RoleNames.AdminRole, RoleNames.CustomerRole, RoleNames.CustomerAdminRole, RoleNames.CustomerAuditorRole)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UsersCreate(RegisterCustomerAuditorUserViewModel model)
        {

            if (ModelState.IsValid)
            {
                AdminUser user = (AdminUser)model.GetUser();

                var result = await userManager.CreateAsync((ApplicationUser)user, model.Password);

                if (result.Succeeded)
                {
                    //var customerAuditor = new CustomerAuditor(null, model.CustomerID, user.Id);
                    //customerAuditorRepository.Create(customerAuditor);

                    //Lo agregamos al Rol "CustomerAuditorRole"
                    customerAuditorRepository.SetRole(user.Id);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    //string code = await userManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    //await userManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
                    await workflowMessageService.SendUserRegistrationMessageAsync(userManager, user.Id, Request.Url.Scheme, Url);

                    //workflowMessageService.SendContractorUserRegistrationNotificationMessage(model);
                    return RedirectToAction("Users", "CustomerAdminAuditors");
                }
                else
                {
                    var errors = string.Join(",", result.Errors);
                    ModelState.AddModelError(string.Empty, errors);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [AuthorizeRoles(RoleNames.AdminRole, RoleNames.CustomerRole, RoleNames.CustomerAdminRole, RoleNames.CustomerAuditorRole)]
        public async Task<ActionResult> AssignedCustomers(string userID)
        {
            var customerAuditors = await customerAuditorRepository.FindCustomerAuditorsByUserIDAsync(userID);

            var user = await userManager.FindByIdAsync(userID);

            var viewModel = new AssignedCustomersViewModel(user, customerAuditors);

            return View(viewModel);
        }

        //[Authorize(Roles = "AdminRole,CustomerRole,CustomerAdminRole, CustomerAuditorRole")]
        [AuthorizeRoles(RoleNames.AdminRole, RoleNames.CustomerRole, RoleNames.CustomerAdminRole, RoleNames.CustomerAuditorRole)]
        public async Task<ActionResult> CustomersCreate(string userID)
        {

            EditAssignCustomerViewModel model = new Models.EditAssignCustomerViewModel();
            model.UserID = userID;

            ViewBag.CustomerID = new SelectList(customerRepository.UnAssignedCustomers(userID), "CustomerID", "CompanyName");
            ViewBag.UserName = model.UserName;

            return await Task.FromResult(View(model));
        }

        [HttpPost]
        //[Authorize(Roles = "AdminRole,CustomerRole,CustomerAdminRole, CustomerAuditorRole")]
        [AuthorizeRoles(RoleNames.AdminRole, RoleNames.CustomerRole, RoleNames.CustomerAdminRole, RoleNames.CustomerAuditorRole)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CustomersCreate(EditAssignCustomerViewModel model)
        {

            if (ModelState.IsValid)
            {
                CustomerAuditor customer = (CustomerAuditor)model.GetCustomer();
                customer.UserId = model.UserID;
                try
                {
                    await customerAuditorRepository.CreateAsync(customer);

                    return RedirectToAction("AssignedCustomers", "CustomerAdminAuditors", new { userID = model.UserID });
                }
                catch (Exception e)
                {
                    var errors = string.Join(",", e.Message);
                    ModelState.AddModelError(string.Empty, errors);
                }
            }

            ViewBag.CustomerID = new SelectList(customerRepository.UnAssignedCustomers(model.UserID), "CustomerID", "CompanyName");
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //[Authorize(Roles = "AdminRole,CustomerRole,CustomerAdminRole, CustomerAuditorRole")]
        [AuthorizeRoles(RoleNames.AdminRole, RoleNames.CustomerRole, RoleNames.CustomerAdminRole, RoleNames.CustomerAuditorRole)]
        public async Task<ActionResult> CustomersDelete(int customerAuditorID, string userID)
        {
            CustomerAuditor customerAuditor = await customerAuditorRepository.FindByIdAsync(customerAuditorID);
            if (customerAuditor == null)
            {
                return HttpNotFound();
            }

            var model = new EditAssignCustomerViewModel(customerAuditor);
            return View(model);
        }

        [HttpPost, ActionName("CustomersDelete")]
        //[Authorize(Roles = "AdminRole,CustomerRole,CustomerAdminRole, CustomerAuditorRole")]
        [AuthorizeRoles(RoleNames.AdminRole, RoleNames.CustomerRole, RoleNames.CustomerAdminRole, RoleNames.CustomerAuditorRole)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CustomersDeleteConfirmed(int customerAuditorID, string userID)
        {
            await customerAuditorRepository.DeleteAsync(customerAuditorID);
            return RedirectToAction("AssignedCustomers", "CustomerAdminAuditors", new { userID = userID });
        }

        // GET: Customers/Delete/5
        //[Authorize(Roles = "AdminRole,CustomerRole,CustomerAdminRole, CustomerAuditorRole")]
        [AuthorizeRoles(RoleNames.AdminRole, RoleNames.CustomerRole, RoleNames.CustomerAdminRole, RoleNames.CustomerAuditorRole)]
        public async Task<ActionResult> UsersDelete(int customerAuditorID, int customerID, string userId)
        {
            if ((userId == null) || (userId.ToString().Length == 0))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //var user = customerAuditorRepository.FindCustomerAuditorsByUserIDAsync(userId);
            //if (user == null)
            //{
            //    return HttpNotFound();
            //}

            //Falta eliminar la relacion del usuario con los Clientes
            //await customerAuditorRepository.DeleteByUserIdAsync(userId);

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return HttpNotFound();
            }

            var userAuditor = new CustomerAuditor() { CustomerAuditorID = customerAuditorID, CustomerID = customerID, User = (AdminUser)user, UserId = userId };

            var model = new EditCustomerAuditorUserViewModel(userAuditor);
            return View(model);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("UsersDelete")]
        //[Authorize(Roles = "AdminRole,CustomerRole,CustomerAdminRole, CustomerAuditorRole")]
        [AuthorizeRoles(RoleNames.AdminRole, RoleNames.CustomerRole, RoleNames.CustomerAdminRole, RoleNames.CustomerAuditorRole)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UsersDeleteConfirmed(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return HttpNotFound();
            }

            await customerAuditorRepository.DeleteByUserIdAsync(userId);

            var result = userManager.Delete(user);
            if (result.Succeeded)
            {
                return RedirectToAction("Users", "CustomerAdminAuditors");
            }

            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (customerAuditorRepository != null)
                {
                    customerAuditorRepository.Dispose();
                    customerAuditorRepository = null;
                }
                
                if (customerRepository != null)
                {
                    customerRepository.Dispose();
                    customerRepository = null;
                }
                
                if (workflowMessageService != null)
                {
                    workflowMessageService.Dispose();
                    workflowMessageService = null;
                }
            }
            base.Dispose(disposing);
        }

    }
}