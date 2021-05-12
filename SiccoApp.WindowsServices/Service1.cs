using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using SiccoApp.Logging;
using SiccoApp.Persistence;

namespace SiccoApp.WindowsServices
{
    public partial class Service1 : ServiceBase
    {
        private Autofac.IContainer container;
        private ILogger logger;

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            logger.Information("SiccoApp.WindowsServices entry point called");

            var builder = new ContainerBuilder();
            builder.RegisterType<Logger>().As<ILogger>().SingleInstance();
            builder.RegisterType<RequirementRepository>().As<IRequirementRepository>();
            container = builder.Build();

            logger = container.Resolve<ILogger>();

            IRequirementRepository req = new RequirementRepository(logger);

            req.GenerateByPeriodAsync(1, 201605, DateTime.UtcNow, null);
        }

        protected override void OnStop()
        {
            base.OnStop();
        }
    }
}
