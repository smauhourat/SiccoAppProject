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

namespace SiccoApp.Controllers
{
    public class CustomerAuditorsController : BaseController
    {
        private ICustomerAuditorRespository customerAuditorRepository = null;
        private IWorkflowMessageService workflowMessageService = null;

        //ojoooo no me gusta
        private UserManager<ApplicationUser> userManager = null;

        public CustomerAuditorsController(ICustomerAuditorRespository customerAuditorRepo, IWorkflowMessageService workflowMessageServ)
        {
            customerAuditorRepository = customerAuditorRepo;
            workflowMessageService = workflowMessageServ;

            userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
        }

        [Authorize(Roles = "AdminRole,CustomerRole,CustomerAdminRole, CustomerAuditorRole")]
        public async Task<ActionResult> Users()
        {
            var customerAuditors = await customerAuditorRepository.FindCustomerAuditorsByCustomerAsync(base.CurrentCustomerID);
            var model = new CustomerAuditorsListViewModel(base.CurrentCustomerID, customerAuditors);

            //return View(customerAuditors.ToList());
            return View(model);
        }

        // GET: CustomerAuditors/UsersCreate
        [Authorize(Roles = "AdminRole,CustomerRole,CustomerAdminRole, CustomerAuditorRole")]
        public ActionResult UsersCreate(int customerID)
        {
            RegisterCustomerAuditorUserViewModel model = new RegisterCustomerAuditorUserViewModel();
            model.CustomerID = customerID;
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "AdminRole,CustomerRole,CustomerAdminRole, CustomerAuditorRole")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult UsersCreate(RegisterCustomerAuditorUserViewModel model)
        {

            if (ModelState.IsValid)
            {
                AdminUser user = (AdminUser)model.GetUser();

                var result = userManager.Create((ApplicationUser)user, model.Password);
                
                if (result.Succeeded)
                {
                    var customerAuditor = new CustomerAuditor(null, base.CurrentCustomerID, user.Id);
                    customerAuditorRepository.Create(customerAuditor);

                    //workflowMessageService.SendContractorUserRegistrationNotificationMessage(model);
                    return RedirectToAction("Users", "CustomerAuditors");
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

        // GET: CustomerAuditors/UsersDelete/5
        [Authorize(Roles = "AdminRole,CustomerRole,CustomerAdminRole, CustomerAuditorRole")]
        public async Task<ActionResult> UsersDelete(int customerAuditorID)
        {
            //if ((customerAuditorID == null) || (username.ToString().Length == 0))
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}

            CustomerAuditor customerAuditor = await customerAuditorRepository.FindByIdAsync(customerAuditorID);
            if (customerAuditor == null)
            {
                return HttpNotFound();
            }

            var model = new EditCustomerAuditorUserViewModel(customerAuditor);
            return View(model);
        }

        // POST: CustomerAuditors/UsersDelete/5
        [Authorize(Roles = "AdminRole,CustomerRole,CustomerAdminRole, CustomerAuditorRole")]
        [HttpPost, ActionName("UsersDelete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UsersDeleteConfirmed(int customerAuditorID)
        {
            CustomerAuditor customerAuditor = await customerAuditorRepository.FindByIdAsync(customerAuditorID);
            if (customerAuditor == null)
            {
                return HttpNotFound();
            }

            await customerAuditorRepository.Delete(customerAuditorID);

            return RedirectToAction("Users", "CustomerAuditors");


        }

        // GET: CustomerAuditors/UsersEdit/5
        [Authorize(Roles = "AdminRole,CustomerRole,CustomerAdminRole, CustomerAuditorRole")]
        public async Task<ActionResult> UsersEdit(int customerAuditorID)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            CustomerAuditor customerAuditor = await customerAuditorRepository.FindByIdAsync(customerAuditorID);
            if (customerAuditor == null)
            {
                return HttpNotFound();
            }

            var model = new EditCustomerAuditorUserViewModel(customerAuditor);
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "AdminRole,CustomerRole,CustomerAdminRole, CustomerAuditorRole")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UsersEdit([Bind(Include = "CustomerAuditorID,UserId,UserName,FirstName,LastName,Email")] EditCustomerAuditorUserViewModel customerAuditorVM)
        {

            if (ModelState.IsValid)
            {
                CustomerAuditor customerAuditor = await customerAuditorRepository.FindByIdAsync(customerAuditorVM.CustomerAuditorID.Value);
                customerAuditor.User.Id = customerAuditorVM.UserId;
                customerAuditor.User.UserName = customerAuditorVM.UserName;
                customerAuditor.User.FirstName = customerAuditorVM.FirstName;
                customerAuditor.User.LastName = customerAuditorVM.LastName;
                customerAuditor.User.Email = customerAuditorVM.Email;

                customerAuditorRepository.Update(customerAuditor);

                return RedirectToAction("Users");
            }
            return View(customerAuditorVM);
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
                
                if (workflowMessageService != null)
                {
                    workflowMessageService.Dispose();
                    workflowMessageService = null;
                }
                
                if (userManager != null)
                {
                    userManager.Dispose();
                    userManager = null;
                }
            }
            base.Dispose(disposing);
        }
    }
}