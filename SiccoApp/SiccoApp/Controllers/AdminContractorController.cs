using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Web.Mvc;
using SiccoApp.Models;
using SiccoApp.Persistence;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity.EntityFramework;

namespace SiccoApp.Controllers
{
    public class AdminContractorController : BaseController
    {
        private IContractorRepository contractorRepository = null;
        private UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public AdminContractorController(IContractorRepository contractorRepo)
        {
            contractorRepository = contractorRepo;
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(AdminContractorLoginViewModel model, string returnUrl)
        {

            if (ModelState.IsValid)
            {
                var user = await userManager.FindAsync(model.UserName, model.Password);

                if (user == null)
                {
                    ModelState.AddModelError("", Resources.Resources.InvalidUsernameOrPassword);
                    return View(model);
                }

                //Add this to check if the email was confirmed.
                if (!await userManager.IsEmailConfirmedAsync(user.Id))
                {
                    //ModelState.AddModelError("", "Necesita confirmar el mail para poder ingresar al Sistema.");
                    ModelState.AddModelError("", Resources.Resources.IsNotEmailConfirmed);
                    return View(model);
                }

                //if (user != null)
                if (user != null && user.GetType().BaseType.Name == typeof(ContractorUser).Name)
                {
                    await SignInAsync(user, model.RememberMe);

                    //base.CurrentCustomerID = model.CustomerID;
                    try
                    {
                        Contractor contractor = await contractorRepository.FindContractorsByIDAsync(((ContractorUser)user).ContractorID);
                        base.CurrentCustomerID = contractor.CustomerID;
                        base.CurrentContractorID = contractor.ContractorID;
                        base.CurrentUserFullname = user.LastName + ", " + user.FirstName;
                        base.CurrentUserID = user.Id;
                        //return RedirectToLocal(returnUrl, "Index", "AdminContractor");
                        return RedirectToAction("Index", returnUrl.Replace("/", ""));
                    }
                    catch //(System.InvalidCastException ex)
                    {
                        ModelState.AddModelError("", Resources.Resources.InvalidUsernameOrPassword);
                    }
                }
                else
                {
                    ModelState.AddModelError("", Resources.Resources.InvalidUsernameOrPassword);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            base.CurrentCustomerID = -1;
            base.CurrentContractorID = -1;
            base.CurrentUserFullname = "";
            base.CurrentUserID = "";
            return RedirectToAction("Index", "AdminContractor");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (userManager != null)
                {
                    userManager.Dispose();
                    userManager = null;
                }

                if (contractorRepository != null)
                {
                    contractorRepository.Dispose();
                    contractorRepository = null;
                }
            }
            base.Dispose(disposing);
        }
    }
}