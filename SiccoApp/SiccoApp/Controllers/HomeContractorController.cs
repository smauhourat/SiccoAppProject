using SiccoApp.Helpers;
using SiccoApp.Models;
using SiccoApp.Persistence;
using SiccoApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SiccoApp.Controllers
{
    public class HomeContractorController : BaseController
    {
        private IContractorRepository contractorRepository = null;
        private IWorkflowMessageService workflowMessageService = null;

        public HomeContractorController(IContractorRepository contractorRepo, IWorkflowMessageService workflowMessageServ)
        {
            contractorRepository = contractorRepo;
            workflowMessageService = workflowMessageServ;

            //ViewBag.UserFullname = base.CurrentUserFullname;
            ViewBag.UserFullname = base.CurrentCustomerID.ToString();
        }

        // GET: HomeContractor
        [AuthorizeRoles(RoleNames.ContractorRole)]
        public async Task<ActionResult> Index()
        {
            Contractor contractor = await contractorRepository.FindContractorsByIDAsync(base.CurrentContractorID);
            if (contractor == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserFullname = base.CurrentUserFullname;

            var model = new ContractorDashboardViewModel(contractor);
            return View(model);
            
            //return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult SetCulture(string culture)
        {
            // Validate input
            culture = CultureHelper.GetImplementedCulture(culture);
            // Save culture in a cookie
            HttpCookie cookie = Request.Cookies["_culture"];
            if (cookie != null)
                cookie.Value = culture;   // update cookie value
            else
            {
                cookie = new HttpCookie("_culture");
                cookie.Value = culture;
                cookie.Expires = DateTime.Now.AddYears(1);
            }
            Response.Cookies.Add(cookie);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (contractorRepository != null)
                {
                    contractorRepository.Dispose();
                    contractorRepository = null;
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
