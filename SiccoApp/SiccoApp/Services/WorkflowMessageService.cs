using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiccoApp.Models;
using System.Web.Script.Serialization;
using SiccoApp.Messaging;
using SiccoApp.Persistence;
using SiccoApp.Logging;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;

namespace SiccoApp.Services
{
    public static class Constants
    {
        public static class Messages
        {
            public const string SubjectConfirmMail = "Confirme su cuenta de Usuario - Sicco";
            public const string BodyConfirmMail = "Por favor confirme su cuenta haciendo click  <a href='{0}'>aqui</a>";
            public const string SubjectResetPassword = "Reestablecimiento de Contraseña - Sicco";
            public const string BodyResetPassword = "Por favor resetee su contraseña haciendo click <a href='{0}'>aqui</a>";
        }

        public static class ActionNames
        {
            public const string ConfirmEmail = "ConfirmEmail";
            public const string ResetPassword = "ResetPassword";
        }

    }

    public class WorkflowMessageService : IWorkflowMessageService
    {
        private ILogger log = null;
        private IEmailManager _emailManager;
        private IEmailAccountRepository _emailAccountRepository = null;

        public WorkflowMessageService(ILogger logger, IEmailManager emailManager, IEmailAccountRepository emailAccountRepo)
        {
            log = logger;
            _emailManager = emailManager;
            _emailAccountRepository = emailAccountRepo;
        }

        #warning "Aca deberia pasarle tambien el template html del body, junto con un listado de tokens a reemplazar en ese template"
        public async Task SendContractorUserRegistrationNotificationMessageAsync(RegisterContractorUserViewModel customerVM)
        {
            if (customerVM == null)
                throw new ArgumentNullException("customer");

            EmailAccount emailAccount = _emailAccountRepository.FindEmailAccountsDefaultAsync();

            //Aca deberia pasarle tambien el template html del body, junto con un listado de tokens a reemplazar en ese template
            string mailMessageBody = "Usuario: " + customerVM.UserName + "</br> Contraseña: " + customerVM.Password;
            await _emailManager.SendSimpleMessageAsync(emailAccount, emailAccount.Email, customerVM.Email, customerVM.LastName + "," + customerVM.FirstName, "Usuario Registrado Sicco", mailMessageBody);

        }

        public async Task SendRequirementPresentationNotificationMessageAsync(PresentationViewModel presentationVM, List<string> mailReceipts)
        {
            try
            {
                EmailAccount emailAccount = _emailAccountRepository.FindEmailAccountsDefaultAsync();

                string mailMessageBody = "Se agrego un documento al requerimiento : " + presentationVM.DocumentationCode + " - " + presentationVM.DocumentationDescription + " // " + presentationVM.ResourceType + " - " + presentationVM.ResourceIdentification + " // -" + presentationVM.PresentationDate;
                foreach (var mail in mailReceipts)
                {
                    await _emailManager.SendSimpleMessageAsync(emailAccount, emailAccount.Email, mail, "", "Presentacion", mailMessageBody);
                }
            }
            catch (Exception e)
            {
                log.Error(e, "Error in WorkflowMessageService.SendRequirementPresentationNotificationMessage(presentationID={0})", presentationVM.PresentationID);
                throw;

            }
        }

        #warning "Reemplazar mail santiagomauhourat@hotmail.com por el del destinatario"
        public async Task SendRequirementPresentationDeleteNotificationMessageAsync(PresentationViewModel presentationVM, List<string> mailReceipts)
        {
            EmailAccount emailAccount = _emailAccountRepository.FindEmailAccountsDefaultAsync();

            string mailMessageBody = "Se elimino una Presentacion al requerimiento : " + presentationVM.DocumentationCode + " - " + presentationVM.DocumentationDescription + " // " + presentationVM.ResourceType + " - " + presentationVM.ResourceIdentification + " // -" + presentationVM.PresentationDate;
            foreach (var mail in mailReceipts)
            {
                //_emailManager.SendSimpleMessage(emailAccount, emailAccount.Email, mail, "", "Presentacion", mailMessageBody);
                await _emailManager.SendSimpleMessageAsync(emailAccount, emailAccount.Email, "santiagomauhourat@hotmail.com", "", "Presentacion - ELIMINADA (" + mail + " - CONTRATISTA)", mailMessageBody);
            }
        }

        #warning "Reemplazar mail santiagomauhourat@hotmail.com por el del destinatario"
        public async Task SendRequirementPresentationApproveNotificationMessageAsync(PresentationViewModel presentationVM, List<string> mailReceipts)
        {
            EmailAccount emailAccount = _emailAccountRepository.FindEmailAccountsDefaultAsync();

            string mailMessageBody = "Se Aprobo el requerimiento : " + presentationVM.DocumentationCode + " - " + presentationVM.DocumentationDescription + " // " + presentationVM.ResourceType + " - " + presentationVM.ResourceIdentification + " // -" + presentationVM.PresentationDate;
            foreach (var mail in mailReceipts)
            {
                //await _emailManager.SendSimpleMessageAsync(emailAccount, emailAccount.Email, mail, "", "Presentacion - APROBADA", mailMessageBody);
                await _emailManager.SendSimpleMessageAsync(emailAccount, emailAccount.Email, "santiagomauhourat@hotmail.com", "", "Presentacion - APROBADA (" + mail + " - CONTRATISTA)", mailMessageBody);
            }
        }
        
        #warning "Reemplazar mail santiagomauhourat@hotmail.com por el del destinatario"
        public async Task SendRequirementPresentationRejectNotificationMessageAsync(PresentationViewModel presentationVM, List<string> mailReceipts)
        {
            EmailAccount emailAccount = _emailAccountRepository.FindEmailAccountsDefaultAsync();

            string mailMessageBody = "Se Rechazo el requerimiento : " + presentationVM.DocumentationCode + " - " + presentationVM.DocumentationDescription + " // " + presentationVM.ResourceType + " - " + presentationVM.ResourceIdentification + " // -" + presentationVM.PresentationDate;
            foreach (var mail in mailReceipts)
            {
                //await _emailManager.SendSimpleMessageAsync(emailAccount, emailAccount.Email, mail, "", "Presentacion - RECHAZADA", mailMessageBody);
                await _emailManager.SendSimpleMessageAsync(emailAccount, emailAccount.Email, "santiagomauhourat@hotmail.com", "", "Presentacion - RECHAZADA (" + mail + " - CONTRATISTA)", mailMessageBody);
            }
        }

        public async Task SendUserRegistrationMessageAsync(UserManager<ApplicationUser> userManager, string userId, string uriScheme, UrlHelper url)
        {
            string code = await userManager.GenerateEmailConfirmationTokenAsync(userId);

            var callbackUrl = url.Action(Constants.ActionNames.ConfirmEmail, "Account", new { userId = userId, code = code }, protocol: uriScheme);
            await userManager.SendEmailAsync(userId, Constants.Messages.SubjectConfirmMail, String.Format(Constants.Messages.BodyConfirmMail, callbackUrl)); //"Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
        }

        public async Task SendUserRegistrationMessageAsync(UserManager<ContractorUser> userManager, string userId, string uriScheme, UrlHelper url)
        {
            string code = await userManager.GenerateEmailConfirmationTokenAsync(userId);

            var callbackUrl = url.Action(Constants.ActionNames.ConfirmEmail, "Account", new { userId = userId, code = code }, protocol: uriScheme);
            await userManager.SendEmailAsync(userId, Constants.Messages.SubjectConfirmMail, String.Format(Constants.Messages.BodyConfirmMail, callbackUrl)); //"Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
        }

        public async Task SendUserRegistrationMessageAsync(UserManager<CustomerUser> userManager, string userId, string uriScheme, UrlHelper url)
        {
            string code = await userManager.GenerateEmailConfirmationTokenAsync(userId);

            var callbackUrl = url.Action(Constants.ActionNames.ConfirmEmail, "Account", new { userId = userId, code = code }, protocol: uriScheme);
            await userManager.SendEmailAsync(userId, Constants.Messages.SubjectConfirmMail, String.Format(Constants.Messages.BodyConfirmMail, callbackUrl)); //"Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
        }

        public async Task SendUserResetPasswordMessageAsync(UserManager<ApplicationUser> userManager, string userId, string uriScheme, UrlHelper url)
        {

            //string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
            //var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
            //await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");

            string code = await userManager.GeneratePasswordResetTokenAsync(userId);

            var callbackUrl = url.Action(Constants.ActionNames.ResetPassword, "Account", new { userId = userId, code = code }, protocol: uriScheme);
            await userManager.SendEmailAsync(userId, Constants.Messages.SubjectResetPassword, String.Format(Constants.Messages.BodyResetPassword, callbackUrl));
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
        }
    }
}
