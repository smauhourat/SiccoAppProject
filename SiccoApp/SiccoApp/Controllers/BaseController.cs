using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using SiccoApp.Helpers;

namespace SiccoApp.Controllers
{
    public class BaseController : Controller, IDisposable 
    {
        private int _currentCustomerID;
        private int _currentContractorID;
        private string _currentUserFullname;
        private string _currentUserID;

        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            string cultureName = null;

            // Attempt to read the culture cookie from Request
            HttpCookie cultureCookie = Request.Cookies["_culture"];
            if (cultureCookie != null)
                cultureName = cultureCookie.Value;
            else
                cultureName = Request.UserLanguages != null && Request.UserLanguages.Length > 0 ? Request.UserLanguages[0] : null; // obtain it from HTTP header AcceptLanguages

            // Validate culture name
            cultureName = CultureHelper.GetImplementedCulture(cultureName); // This is safe


            // Modify current thread's cultures            
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cultureName);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;


            HttpCookie currentCustomerIDCookie = Request.Cookies["_customerID"];
            if (currentCustomerIDCookie != null)
                _currentCustomerID = Int32.Parse(currentCustomerIDCookie.Value);

            HttpCookie currentContractorIDCookie = Request.Cookies["_contractorID"];
            if (currentContractorIDCookie != null)
                _currentContractorID = Int32.Parse(currentContractorIDCookie.Value);

            HttpCookie currentUserFullnameCookie = Request.Cookies["_currentUserFullname"];
            if (currentUserFullnameCookie != null)
                _currentUserFullname = currentUserFullnameCookie.Value;

            HttpCookie currentUserIDCookie = Request.Cookies["_currentUserID"];
            if (currentUserIDCookie != null)
                _currentUserID = currentUserIDCookie.Value;

            return base.BeginExecuteCore(callback, state);
        }

        public int CurrentCustomerID
        {
            get { return _currentCustomerID;  }
            set
            {
                // Save culture in a cookie
                HttpCookie cookie = Request.Cookies["_customerID"];
                if (cookie != null)
                    cookie.Value = value.ToString();   // update cookie value
                else
                {
                    cookie = new HttpCookie("_customerID");
                    cookie.Value = value.ToString();
                    cookie.Expires = DateTime.Now.AddYears(1);
                }

                _currentCustomerID = Int32.Parse(cookie.Value);

                Response.Cookies.Add(cookie);
            }
        }

        public int CurrentContractorID
        {
            get { return _currentContractorID; }
            set
            {
                // Save culture in a cookie
                HttpCookie cookie = Request.Cookies["_contractorID"];
                if (cookie != null)
                    cookie.Value = value.ToString();   // update cookie value
                else
                {
                    cookie = new HttpCookie("_contractorID");
                    cookie.Value = value.ToString();
                    cookie.Expires = DateTime.Now.AddYears(1);
                }

                _currentCustomerID = Int32.Parse(cookie.Value);

                Response.Cookies.Add(cookie);
            }
        }

        public string CurrentUserFullname
        {
            get { return _currentUserFullname; }
            set
            {
                // Save culture in a cookie
                HttpCookie cookie = Request.Cookies["_currentUserFullname"];
                if (cookie != null)
                    cookie.Value = value.ToString();   // update cookie value
                else
                {
                    cookie = new HttpCookie("_currentUserFullname");
                    cookie.Value = value.ToString();
                    cookie.Expires = DateTime.Now.AddYears(1);
                }

                _currentUserFullname = cookie.Value.ToString();

                Response.Cookies.Add(cookie);
            }
        }
        
        public string CurrentUserID
        {
            get { return _currentUserID; }
            set
            {
                // Save culture in a cookie
                HttpCookie cookie = Request.Cookies["_currentUserID"];
                if (cookie != null)
                    cookie.Value = value.ToString();   // update cookie value
                else
                {
                    cookie = new HttpCookie("_currentUserID");
                    cookie.Value = value.ToString();
                    cookie.Expires = DateTime.Now.AddYears(1);
                }

                _currentUserID = cookie.Value.ToString();

                Response.Cookies.Add(cookie);
            }
        }

        public void CleanCookies()
        {
            Response.Cookies.Remove("_customerID");
            Response.Cookies.Remove("_contractorID");
            Response.Cookies.Remove("_currentUserFullname");
            Response.Cookies.Remove("_currentUserID");
        }

        //private void SetCustomer(int customerID)
        //{
        //    // Save culture in a cookie
        //    HttpCookie cookie = Request.Cookies["_customerID"];
        //    if (cookie != null)
        //        cookie.Value = customerID.ToString();   // update cookie value
        //    else
        //    {
        //        cookie = new HttpCookie("_customerID");
        //        cookie.Value = customerID.ToString();
        //        cookie.Expires = DateTime.Now.AddYears(1);
        //    }

        //    _currentCustomerID = Int32.Parse(cookie.Value);

        //    Response.Cookies.Add(cookie);
        //}

        public ActionResult RedirectToLocal(string returnUrl, string defaultActionName, string defaultRoutesValues)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                //return RedirectToAction("Index", "AdminCustomer");
                return RedirectToAction(defaultActionName, defaultRoutesValues);
            }
        }
    }
}