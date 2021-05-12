using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
//using SiccoApp.DAL;
using SiccoApp.Persistence;
using System.Threading.Tasks;
using Resources;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SiccoApp.Models;
using SiccoApp.Helpers;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.AspNet.Identity.Owin;
using SiccoApp.Services;

namespace SiccoApp.Controllers
{
    public class CustomersController : BaseController
    {
        private ICustomerRepository customerRepository = null;
        private ILocalizationRepository localizationRepository = null;
        private IIdentityMessageService emailService = null;
        private IWorkflowMessageService workflowMessageService = null;

        //ojoooo no me gusta
        private UserManager<CustomerUser> userManager = null;


        public CustomersController(ICustomerRepository customerRepo, 
            ILocalizationRepository localizationRepo, 
            IWorkflowMessageService workflowMessageServ,
            IIdentityMessageService emailServ)
        {
            customerRepository = customerRepo;
            localizationRepository = localizationRepo;

            emailService = emailServ;

            workflowMessageService = workflowMessageServ;

            userManager = new UserManager<CustomerUser>(new UserStore<CustomerUser>(new ApplicationDbContext()));
            userManager.UserValidator = new UserValidator<CustomerUser>(userManager)
            {
                AllowOnlyAlphanumericUserNames = true,
                RequireUniqueEmail = true
            };

            var provider = new DpapiDataProtectionProvider("SampleAppName");
            userManager.UserTokenProvider = new DataProtectorTokenProvider<CustomerUser>(provider.Create("SampleTokenName"));
            userManager.EmailService = emailService;
        }

        //[Authorize(Roles = "AdminRole,CustomerRole,CustomerAdminRole, CustomerAuditorRole")]
        [AuthorizeRoles(RoleNames.AdminRole)]
        public async Task<ActionResult> Index()
        {
            var customers = await customerRepository.FindCustomersAsync();
            return View(customers.ToList());
        }

        // GET: Customers/Details/5
        //[Authorize(Roles = "AdminRole,CustomerRole,CustomerAdminRole, CustomerAuditorRole")]
        [AuthorizeRoles(RoleNames.AdminRole)]
        public async Task<ActionResult> Details(int id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            Customer customer = await customerRepository.FindCustomerByIDAsync(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: Customers/Create
        //[Authorize(Roles = "AdminRole,CustomerRole,CustomerAdminRole, CustomerAuditorRole")]
        [AuthorizeRoles(RoleNames.AdminRole)]
        public ActionResult Create()
        {
            ViewBag.CountryID = new SelectList(localizationRepository.Countries(), "CountryID", "CountryName");
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[Authorize(Roles = "AdminRole,CustomerRole,CustomerAdminRole, CustomerAuditorRole")]
        [AuthorizeRoles(RoleNames.AdminRole)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "CustomerID,CompanyName,TaxIdNumber,CountryID,StateID,City,Address,PhoneNumber,Active,CreationDate,CreationUser,ModifiedDate,ModifiedUser")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                //var currentUsername = !string.IsNullOrEmpty(System.Web.HttpContext.Current?.User?.Identity?.Name) ? HttpContext.Current.User.Identity.Name : "Anonymous";
                customer.CreationDate = DateTime.UtcNow;
                customer.CreationUser = System.Web.HttpContext.Current.User.Identity.Name;
                await customerRepository.CreateAsync(customer);
                return RedirectToAction("Index");
            }

            ViewBag.CountryID = new SelectList(localizationRepository.Countries(), "CountryID", "CountryName", customer.CountryID);
            return View(customer);
        }

        // GET: Customers/Edit/5
        //[Authorize(Roles = "AdminRole,CustomerRole,CustomerAdminRole, CustomerAuditorRole")]
        [AuthorizeRoles(RoleNames.AdminRole)]
        public async Task<ActionResult> Edit(int id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            Customer customer = await customerRepository.FindCustomerByIDAsync(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            ViewBag.CountryID = new SelectList(localizationRepository.Countries(), "CountryID", "CountryName", customer.CountryID);
            ViewBag.StateID = new SelectList(localizationRepository.States(customer.CountryID), "StateID", "StateName", customer.StateID);
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[Authorize(Roles = "AdminRole,CustomerRole,CustomerAdminRole, CustomerAuditorRole")]
        [AuthorizeRoles(RoleNames.AdminRole)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "CustomerID,CompanyName,TaxIdNumber,CountryID,StateID,City,Address,PhoneNumber,Active,CreationDate,CreationUser,ModifiedDate,ModifiedUser")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                customer.ModifiedDate = DateTime.UtcNow;
                customer.ModifiedUser = System.Web.HttpContext.Current.User.Identity.Name;
                await customerRepository.UpdateAsync(customer);

                return RedirectToAction("Index");
            }
            ViewBag.CountryID = new SelectList(localizationRepository.Countries(), "CountryID", "CountryName", customer.CountryID);
            ViewBag.StateID = new SelectList(localizationRepository.States(customer.CountryID), "StateID", "StateName", customer.StateID);
            return View(customer);
        }

        // GET: Customers/Delete/5
        //[Authorize(Roles = "AdminRole,CustomerRole,CustomerAdminRole, CustomerAuditorRole")]
        [AuthorizeRoles(RoleNames.AdminRole)]
        public async Task<ActionResult> Delete(int id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            Customer customer = await customerRepository.FindCustomerByIDAsync(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        //[Authorize(Roles = "AdminRole,CustomerRole,CustomerAdminRole, CustomerAuditorRole")]
        [AuthorizeRoles(RoleNames.AdminRole)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await customerRepository.DeleteAsync(id);
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ErrorMessageParser.Parse(ex.Message));
                //ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        // GET: Customers/Users/5
        //[Authorize(Roles = "AdminRole,CustomerRole,CustomerAdminRole, CustomerAuditorRole")]
        [AuthorizeRoles(RoleNames.AdminRole)]
        public async Task<ActionResult> Users(int id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            Customer customer = await customerRepository.FindCustomerByIDAsync(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            var model = new CustomerUsersViewModel(customer);
            return View(model);
        }

        // GET: Customers/UsersCreate
        //[Authorize(Roles = "AdminRole,CustomerRole,CustomerAdminRole, CustomerAuditorRole")]
        [AuthorizeRoles(RoleNames.AdminRole)]
        public ActionResult UsersCreate(int customerID)
        {
            RegisterCustomerUserViewModel model = new RegisterCustomerUserViewModel();
            model.CustomerID = customerID;
            return View(model);
        }

        [HttpPost]
        //[Authorize(Roles = "AdminRole,CustomerRole,CustomerAdminRole, CustomerAuditorRole")]
        [AuthorizeRoles(RoleNames.AdminRole)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UsersCreate(RegisterCustomerUserViewModel model)
        {
            if ((ModelState.IsValid) && model.CustomerID > 0)
            {
                CustomerUser user = (CustomerUser)model.GetUser();
                user.CustomerID = model.CustomerID;

                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    var resultRole = userManager.AddToRole(user.Id, "CustomerRole");
                    userManager.AddToRole(user.Id, "CustomerAdminRole");

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    //string code = await userManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    //await userManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
                    await workflowMessageService.SendUserRegistrationMessageAsync(userManager, user.Id, Request.Url.Scheme, Url);

                    return RedirectToAction("Users", "Customers", new { id = model.CustomerID });
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

        // GET: Customers/Delete/5
        //[Authorize(Roles = "AdminRole,CustomerRole,CustomerAdminRole, CustomerAuditorRole")]
        [AuthorizeRoles(RoleNames.AdminRole)]
        public ActionResult UsersDelete(string username, int customerID)
        {
            if ((username == null) || (username.ToString().Length == 0))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CustomerUser customeruser = userManager.FindByName(username);
            if (customeruser == null)
            {
                return HttpNotFound();
            }

            var model = new EditCustomerUserViewModel((CustomerUser)customeruser);
            model.CustomerID = customerID;
            return View(model);
        }

        // POST: Customers/Delete/5
        //[Authorize(Roles = "AdminRole,CustomerRole,CustomerAdminRole, CustomerAuditorRole")]
        [AuthorizeRoles(RoleNames.AdminRole)]
        [HttpPost, ActionName("UsersDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult UsersDeleteConfirmed(string username)
        {
            CustomerUser customeruser = userManager.FindByName(username);
            if (customeruser == null)
            {
                return HttpNotFound();
            }

            var result = userManager.Delete(customeruser);
            if (result.Succeeded)
            {
                return RedirectToAction("Users", "Customers", new { id = customeruser.CustomerID });
            }
            else
            {
                var errors = string.Join(",", result.Errors);
                ModelState.AddModelError(string.Empty, errors);
            }

            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);

        }

        // GET: Customers/GenerateDocumentationMatrix/5
        //[Authorize(Roles = "AdminRole,CustomerRole,CustomerAdminRole, CustomerAuditorRole")]
        [AuthorizeRoles(RoleNames.AdminRole)]
        public async Task<ActionResult> GenerateDocumentationMatrix(int customerID)
        {
            Customer customer = await customerRepository.FindCustomerByIDAsync(customerID);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/GenerateDocumentationMatrix/5
        //[Authorize(Roles = "AdminRole,CustomerRole,CustomerAdminRole, CustomerAuditorRole")]
        [AuthorizeRoles(RoleNames.AdminRole)]
        [HttpPost, ActionName("GenerateDocumentationMatrix")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> GenerateDocumentationMatrixConfirmed(int customerID)
        {
            try
            {
                await customerRepository.GenerateDocumentationMatrix(customerID, true);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ErrorMessageParser.Parse(ex.Message));
                return View();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (customerRepository != null)
                {
                    customerRepository.Dispose();
                }

                if (localizationRepository != null)
                {
                    localizationRepository.Dispose();
                    localizationRepository = null;
                }

                if (userManager != null)
                {
                    userManager.Dispose();
                    userManager = null;
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
