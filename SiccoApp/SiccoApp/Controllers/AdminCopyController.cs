using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SiccoApp.Models;
using SiccoApp.Persistence;

namespace SiccoApp.Controllers
{
    public class AdminCopyController : BaseController
    {
        private ICustomerRepository customerRepository = null;
        private UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public AdminCopyController(ICustomerRepository customerRepo)
        {
            customerRepository = customerRepo;
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.CustomerID = new SelectList(customerRepository.Customers(), "CustomerID", "CompanyName");
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(AdminLoginViewModel model, string returnUrl)
        {

            if (ModelState.IsValid)
            {
                var user = await userManager.FindAsync(model.UserName, model.Password);
                if (user != null)
                {

                    await SignInAsync(user, model.RememberMe);
                    //var id = user.Id
                    //base.SetCustomer(model.CustomerID);
                    base.CurrentCustomerID = model.CustomerID;
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    ModelState.AddModelError("", Resources.Resources.InvalidUsernameOrPassword);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            Session.Abandon();

            //base.SetCustomer(-1);
            base.CurrentCustomerID = -1;
            return RedirectToAction("Index", "Admin");
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        //[AllowAnonymous]
        //[Authorize(Roles = "AdminRole, CustomerAdminRole")]
        public ActionResult Index()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (customerRepository != null)
                {
                    customerRepository.Dispose();
                    customerRepository = null;
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
