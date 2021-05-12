using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using SiccoApp.Logging;
using SiccoApp.Persistence;
using SiccoApp.Services;
using Autofac.Core;
using SiccoApp.Messaging;
using SiccoApp.Persistence;
using System.Data.Entity;
using SiccoApp.Persistence.Repositories;
using Hangfire;
using Microsoft.AspNet.Identity;

namespace SiccoApp.App_Start
{
    public class DependenciesConfig
    {
        public static void RegisterDependencies()
        {
            var builder = new ContainerBuilder();

            //builder.RegisterType<EfRepository>().As<IRepository>();
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterType<LogRecordRepository>().As<ILogRecordRepository>();
            builder.RegisterType<DefaultLogger>().As<ILogger>().SingleInstance();
            //builder.RegisterType<Logger>().As<ILogger>().SingleInstance();
            builder.RegisterType<EmailManager>().As<IEmailManager>();
            builder.RegisterType<EmailAccountRepository>().As<IEmailAccountRepository>();
            builder.RegisterType<MailMessage>().As<IMailMessage>();
            builder.RegisterType<EmployeeRelationshipTypeRepository>().As<IEmployeeRelationshipTypeRepository>();
            builder.RegisterType<WorkflowMessageService>().As<IWorkflowMessageService>().WithParameter(ResolvedParameter.ForNamed<IEmailManager>(""))
                .InstancePerLifetimeScope();
            builder.RegisterType<CustomerAuditorRespository>().As<ICustomerAuditorRespository>();
            builder.RegisterType<CustomerRepository>().As<ICustomerRepository>();
            builder.RegisterType<LocalizationRepository>().As<ILocalizationRepository>();
            builder.RegisterType<ContractorRepository>().As<IContractorRepository>();
            builder.RegisterType<ContractRepository>().As<IContractRepository>();
            builder.RegisterType<PeriodRepository>().As<IPeriodRepository>();
            builder.RegisterType<BusinessTypeRepository>().As<IBusinessTypeRepository>();
            builder.RegisterType<EmployeeRepository>().As<IEmployeeRepository>();
            builder.RegisterType<VehicleRepository>().As<IVehicleRepository>();
            builder.RegisterType<BusinessTypeTemplateRepository>().As<IBusinessTypeTemplateRepository>();
            builder.RegisterType<DocumentationTemplateRepository>().As<IDocumentationTemplateRepository>();
            builder.RegisterType<DocumentationRepository>().As<IDocumentationRepository>();
            builder.RegisterType<DocumentationPeriodicityRepository>().As<IDocumentationPeriodicityRepository>();
            builder.RegisterType<EntityTypeRepository>().As<IEntityTypeRepository>();
            builder.RegisterType<DocumentationBusinessTypeTemplateRepository>().As<IDocumentationBusinessTypeTemplateRepository>();
            builder.RegisterType<DocumentationBusinessTypeRepository>().As<IDocumentationBusinessTypeRepository>();
            builder.RegisterType<RequirementRepository>().As<IRequirementRepository>();
            builder.RegisterType<PresentationRepository>().As<IPresentationRepository>();
            builder.RegisterType<DocumentFileService>().As<IDocumentFileService>().SingleInstance();
            //builder.RegisterType<FixItQueueManager>().As<IFixItQueueManager>();
            builder.RegisterType<PresentationServices>().As<IPresentationServices>().WithParameter(ResolvedParameter.ForNamed<IPresentationRepository>(""))
                .WithParameter(ResolvedParameter.ForNamed<ICustomerAuditorRespository>(""))
                .WithParameter(ResolvedParameter.ForNamed<IWorkflowMessageService>(""))
                .InstancePerLifetimeScope();

            //builder.RegisterType<AlertServices>().As<IAlertServices>();
            //builder.RegisterType<AlertServices>().As<IAlertServices>().WithParameter(ResolvedParameter.ForNamed<IRequirementRepository>(""))
            //    .WithParameter(ResolvedParameter.ForNamed<IEmailManager>(""))
            //    .WithParameter(ResolvedParameter.ForNamed<IEmailAccountRepository>(""))
            //    .InstancePerLifetimeScope();

            builder.RegisterType<AlertServices>().As<IAlertServices>().InstancePerLifetimeScope();

            builder.RegisterType<DocumentationImportanceRepository>().As<IDocumentationImportanceRepository>();

            builder.RegisterType<RequirementService>().As<IRequirementService>();

            builder.RegisterType<EmailService>().As<IIdentityMessageService>().WithParameter(ResolvedParameter.ForNamed<IEmailManager>(""))
                .InstancePerLifetimeScope();

            //builder.RegisterGeneric(typeof(Persistence.EfRepository<>)).As(typeof(IRepository<>));


            //builder.RegisterType<SiccoAppContext>()
            //       .As<DbContext>()
            //       .InstancePerRequest();

            //builder.RegisterType<SiccoAppContext>()
            //       .AsImplementedInterfaces()
            //       .InstancePerLifetimeScope();

            //builder.RegisterType<SiccoAppContext>().As<ISiccoAppContext>();

            //builder.RegisterGeneric(typeof(EfRepository<>)).As(typeof(IRepository<>)).InstancePerRequest();

            //builder.RegisterGeneric(typeof(EfRepository<>)).As(typeof(IRepository<>)).WithParameter(ResolvedParameter.ForNamed<SiccoAppContext>(""));

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            GlobalConfiguration.Configuration.UseAutofacActivator(container);
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            // Integration with Hangfire
            //GlobalConfiguration.Configuration.UseActivator(new ContainerJobActivator(container));
            //GlobalConfiguration.Configuration.UseAutofacActivator(container);
        }
    }
}
