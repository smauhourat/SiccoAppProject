using Autofac;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using SiccoApp.Logging;
using SiccoApp.Messaging;

namespace SiccoApp.WorkerRole
{
    public class WorkerRole : RoleEntryPoint
    {
        private IContainer container;
        private ILogger logger;
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);

        public override void Run()
        {
            Trace.TraceInformation("SiccoApp.WorkerRole is running");

            try
            {
                this.RunAsync(this.cancellationTokenSource.Token).Wait();
            }
            finally
            {
                this.runCompleteEvent.Set();
            }
        }

        //public override void Run(CancellationToken token)
        //{
        //    using (var scope = container.BeginLifetimeScope())
        //    {
        //        IEmailManager emailManager = scope.Resolve<IEmailManager>();
        //        try
        //        {
        //            emailManager.ProcessMessages(token);
        //        }
        //        catch (Exception ex)
        //        {
        //            logger.Error(ex, "Exception in worker role Run loop.");
        //        }
        //    }
        //}

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            using (var scope = container.BeginLifetimeScope())
            {
                IEmailManager emailManager = scope.Resolve<IEmailManager>();
                try
                {
                    await emailManager.ProcessMessagesAsync(cancellationToken);
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "Exception in worker role Run loop.");
                }
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            bool result = base.OnStart();

            //Trace.TraceInformation("SiccoApp.WorkerRole has been started");
            var builder = new ContainerBuilder();
            builder.RegisterType<Logger>().As<ILogger>().SingleInstance();
            builder.RegisterType<MailMessage>().As<IMailMessage>();
            builder.RegisterType<EmailManager>().As<IEmailManager>();
            container = builder.Build();


            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("SiccoApp.WorkerRole is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();

            base.OnStop();

            Trace.TraceInformation("SiccoApp.WorkerRole has stopped");
        }

    }
}
