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
using System.Net.Mail;
using System.Net;
using SendGrid.Helpers.Mail;
using SiccoApp.Persistence;
using SiccoApp.Logging;

namespace SiccoApp.Messaging
{
    public class EmailManager : IEmailManager
    {
        private IMailMessage _mailMessage;
        private ILogger log = null;

        public EmailManager(IMailMessage mailMessage, ILogger logger)
        {
            _mailMessage = mailMessage;
            log = logger;
        }

        public void SendEmail(string subject, string body, IMailMessage from, IMailMessage to, IEnumerable<string> bcc, IEnumerable<string> cc)
        {
            throw new NotImplementedException();
        }

        public void SendSimpleMessage(EmailAccount emailAccount, string emailFrom, string emailTo, string recipientDisplayName, string subject, string body)
        {
            try
            {
                var message = new System.Net.Mail.MailMessage();
                message.From = new MailAddress(emailFrom, emailFrom);
                message.To.Add(new MailAddress(emailTo, emailTo));

                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = true;

                //send email
                using (var smtpClient = new SmtpClient())
                {
                    smtpClient.UseDefaultCredentials = emailAccount.UseDefaultCredentials;
                    smtpClient.Host = emailAccount.Host;
                    smtpClient.Port = emailAccount.Port;
                    smtpClient.EnableSsl = emailAccount.EnableSsl;

                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

                    smtpClient.Credentials = emailAccount.UseDefaultCredentials ?
                        CredentialCache.DefaultNetworkCredentials :
                        new NetworkCredential(emailAccount.Username, emailAccount.Password);

                    smtpClient.Send(message);
                    //await smtpClient.SendMailAsync(message);
                }
            }
            catch (Exception e)
            {
                log.Error(e, "Error in EmailManager.SendSimpleMessage()");
                throw;
            }
        }

        public async Task SendSimpleMessageAsync(EmailAccount emailAccount, string emailFrom, string emailTo, string recipientDisplayName, string subject, string body)
        {
            try
            {
                var message = new System.Net.Mail.MailMessage();
                message.From = new MailAddress(emailFrom, emailFrom);
                message.To.Add(new MailAddress(emailTo, emailTo));

                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = true;

                //send email
                using (var smtpClient = new SmtpClient())
                {
                    smtpClient.UseDefaultCredentials = emailAccount.UseDefaultCredentials;
                    smtpClient.Host = emailAccount.Host;
                    smtpClient.Port = emailAccount.Port;
                    smtpClient.EnableSsl = emailAccount.EnableSsl;

                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

                    smtpClient.Credentials = emailAccount.UseDefaultCredentials ?
                        CredentialCache.DefaultNetworkCredentials :
                        new NetworkCredential(emailAccount.Username, emailAccount.Password);

                    await smtpClient.SendMailAsync(message);
                }
            }
            catch (Exception e)
            {
                log.Error(e, "Error in EmailManager.SendSimpleMessageAsync()");
                throw;
            }
        }

        public async Task ProcessMessagesAsync(CancellationToken token)
        {
            throw new NotImplementedException();
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
            }
        }
    }
}
