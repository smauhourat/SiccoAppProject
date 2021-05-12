using Microsoft.AspNet.Identity;
using SiccoApp.Messaging;
using SiccoApp.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SiccoApp
{
    public class EmailService : IIdentityMessageService, IDisposable
    {
        private IEmailManager _emailManager;
        private IEmailAccountRepository _emailAccountRepository = null;

        public EmailService(IEmailManager emailManager, IEmailAccountRepository emailAccountRepo)
        {
            _emailManager = emailManager;
            _emailAccountRepository = emailAccountRepo;

        }

        public async Task SendAsync(IdentityMessage message)
        {
            EmailAccount emailAccount = _emailAccountRepository.FindEmailAccountsDefaultAsync();

            await _emailManager.SendSimpleMessageAsync(emailAccount, emailAccount.Email, message.Destination, message.Destination, message.Subject, message.Body);
            // Plug in your email service here to send an email.
            //return Task.FromResult(0);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    if (_emailManager != null)
                    {
                        _emailManager.Dispose();
                        _emailManager = null;
                    }

                    if (_emailAccountRepository != null)
                    {
                        _emailAccountRepository.Dispose();
                        _emailAccountRepository = null;
                    }
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~EmailService() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion

    }
}