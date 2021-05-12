using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using SendGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SendGrid.Helpers.Mail;
using SiccoApp.Persistence;

namespace SiccoApp.Messaging
{
    public class EmailManagerQueue : IEmailManager
    {
        private IMailMessage _mailMessage;
        private CloudQueueClient _queueClient;
        private static readonly string mailQueueName = "siccomailqueue";
        public EmailManagerQueue(IMailMessage mailMessage)
        {
            _mailMessage = mailMessage;
            var account = CloudStorageAccount.DevelopmentStorageAccount;
            _queueClient = account.CreateCloudQueueClient();
        }

        public void SendEmail(string subject, string body, IMailMessage from, IMailMessage to, IEnumerable<string> bcc, IEnumerable<string> cc)
        {
            throw new NotImplementedException();
        }

        public void SendSimpleMessage(EmailAccount emailAccount, string emailFrom, string emailTo, string recipientDisplayName, string subject, string body)
        {
            CloudQueue queue = _queueClient.GetQueueReference(mailQueueName);
            queue.CreateIfNotExistsAsync();

            _mailMessage.From = emailFrom;
            _mailMessage.To = emailTo;
            _mailMessage.Subject = subject;
            _mailMessage.Body = body;

            string message = JsonConvert.SerializeObject(_mailMessage);

            queue.AddMessage(new CloudQueueMessage(message));

        }

        public Task SendSimpleMessageAsync(EmailAccount emailAccount, string emailFrom, string emailTo, string recipientDisplayName, string subject, string body)
        {
            throw new NotImplementedException();
        }

        public async Task ProcessMessagesAsync(CancellationToken token)
        {
            CloudQueue queue = _queueClient.GetQueueReference(mailQueueName);
            await queue.CreateIfNotExistsAsync();

            while (!token.IsCancellationRequested)
            {
                // The default timeout is 90 seconds, so we won’t continuously poll the queue if there are no messages.
                // Pass in a cancellation token, because the operation can be long-running.
                CloudQueueMessage message = await queue.GetMessageAsync(token);
                if (message != null)
                {
                    IMailMessage mailMessage = JsonConvert.DeserializeObject<MailMessage>(message.AsString);

                    string apiKey = "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";
                    dynamic sg = new SendGridAPIClient(apiKey);

                    Email from = new Email(mailMessage.From);
                    string subject = mailMessage.Subject;
                    Email to = new Email(mailMessage.To);

                    Content content = new Content("text/html", mailMessage.Body);
                    Mail mail = new Mail(from, subject, to, content);
                    //mail.TemplateId = "451f5afd-498d-4117-af97-e815cdb98560";
                    //mail.Personalization[0].AddSubstitution("-name", model.Name);
                    //mail.Personalization[0].AddSubstitution("-pwd-", model.Password);

                    dynamic response = await sg.client.mail.send.post(requestBody: mail.Get());

                    queue.DeleteMessage(message);
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Free managed resources
                if (_queueClient != null)
                {
                    //_queueClient.Dispose();
                    _queueClient = null;
                }
            }
        }
    }
}
