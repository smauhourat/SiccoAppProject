using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using SiccoApp.App_Start;
using SiccoApp.Logging;
using SiccoApp.Persistence;
using System.Data.Entity;

namespace SiccoApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            DependenciesConfig.RegisterDependencies();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            DocumentFileService documentFileService = new DocumentFileService(new Logger());
            //OJOOO activar
            documentFileService.CreateAndConfigureAsync();

            DbConfiguration.SetConfiguration(new SiccoApp.Persistence.SiccoAppConfiguration());
        }
    }
}
