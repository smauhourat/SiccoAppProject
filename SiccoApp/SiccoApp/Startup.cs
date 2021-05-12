using Hangfire;
using Hangfire.Storage;
using Microsoft.Owin;
using Owin;
using SiccoApp.Models;
using SiccoApp.Services;
using System;

[assembly: OwinStartupAttribute(typeof(SiccoApp.Startup))]
namespace SiccoApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

           //Setting SQL connection string for hangfire job's to persist
            GlobalConfiguration.Configuration.UseSqlServerStorage("SiccoAppContext");

            //Delete all jobs, only on debug enviroment
            using (var connection = JobStorage.Current.GetConnection())
            {
                foreach (var recurringJob in connection.GetRecurringJobs())
                {
                    RecurringJob.RemoveIfExists(recurringJob.Id);
                }
            }
            
            //Setting Dashboard url, and put security (only super admin users)
            app.UseHangfireDashboard("/myHangfireDashboard", new DashboardOptions()
            {
                Authorization = new[] { new HangfireAuthorizationFilter() }
            });
            //BackgroundJob.Enqueue(() => Console.WriteLine("Fire-and-forget"));
            //RecurringJob.AddOrUpdate(() => Console.WriteLine("Recurring!"), Cron.Minutely);

            //Daily Job, sending requirements expired alerts
            //RecurringJob.AddOrUpdate<IAlertServices>(x => x.SendMailDueDateRequirements(), Cron.Minutely);

            //Daily Job, sending processed presentation to Contractors
            //RecurringJob.AddOrUpdate<IAlertServices>(x => x.SendMailProcessedPresentations(), Cron.Minutely);

            //DailyJob to generate Requirements for all Contractors (Employee and Vehicle too)
            //RecurringJob.AddOrUpdate<IRequirementService>(x => x.GenerateContractorAllRequirements(), Cron.Minutely);

            app.UseHangfireServer();
        }
    }
}
