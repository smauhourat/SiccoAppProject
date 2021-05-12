using SiccoApp.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SiccoApp.Messaging
{
    public interface IEmailManager : IDisposable
    {
        void SendEmail(string subject, string body, IMailMessage from, IMailMessage to, IEnumerable<string> bcc, IEnumerable<string> cc);
        void SendSimpleMessage(EmailAccount emailAccount, string emailFrom, string emailTo, string recipientDisplayName, string subject, string body);
        Task SendSimpleMessageAsync(EmailAccount emailAccount, string emailFrom, string emailTo, string recipientDisplayName, string subject, string body);
        Task ProcessMessagesAsync(CancellationToken token);
    }
}
