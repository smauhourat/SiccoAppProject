using SiccoApp.Helpers;
using SiccoApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiccoApp.Controllers
{
    public class HomeAdminController : BaseController
    {
        private IAlertServices alertServices = null;

        public HomeAdminController(IAlertServices alertServ)
        {
            alertServices = alertServ;
        }

        [Authorize(Roles = "AdminRole,CustomerAdminRole")]
        public ActionResult Index()
        {
            //alertServices.SendMailDueDateRequirements();
            return View();
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
                if (alertServices != null)
                {
                    alertServices.Dispose();
                    alertServices = null;
                }
            }
            base.Dispose(disposing);
        }
    }
}