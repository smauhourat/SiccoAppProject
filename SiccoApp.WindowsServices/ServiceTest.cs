using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using SiccoApp.Logging;
using SiccoApp.Persistence;

namespace SiccoApp.WindowsServices
{
    public class ServiceTest
    {
        private Autofac.IContainer container;
        private ILogger logger;

        public void Start()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<Logger>().As<ILogger>().SingleInstance();
            builder.RegisterType<RequirementRepository>().As<IRequirementRepository>();
            container = builder.Build();

            logger = container.Resolve<ILogger>();

            logger.Information("SiccoApp.WindowsServices entry point called");

            IRequirementRepository req = new RequirementRepository(logger);

            req.GenerateByPeriodAsync(1, 201605, DateTime.UtcNow, null);
        }
    }
}
