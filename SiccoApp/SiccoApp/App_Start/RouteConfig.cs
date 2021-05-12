using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SiccoApp
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                //defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
                defaults: new { controller = "HomeSelector", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute("GetStatesByCountryId",
                            "customers/getstatesbycountryid/",
                            new { controller = "Customers", action = "GetStatesByCountryId" },
                            new[] { "SiccoApp.Controllers" }
            );

            routes.MapRoute("GetStatesByCountryId2",
                            "../address/getstatesbycountryid2/",
                            new { controller = "Address", action = "GetStatesByCountryId2" },
                            new[] { "SiccoApp.Controllers" }
            );
        }
    }
}
