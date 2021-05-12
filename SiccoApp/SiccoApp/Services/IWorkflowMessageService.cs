using Microsoft.AspNet.Identity;
using SiccoApp.Models;
using SiccoApp.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SiccoApp.Services
{
    public interface IWorkflowMessageService : IDisposable
    {
        Task SendContractorUserRegistrationNotificationMessageAsync(RegisterContractorUserViewModel customer);
        Task SendRequirementPresentationNotificationMessageAsync(PresentationViewModel requirementVM, List<string> mailReceipts);
        Task SendRequirementPresentationDeleteNotificationMessageAsync(PresentationViewModel requirementVM, List<string> mailReceipts);
        Task SendRequirementPresentationApproveNotificationMessageAsync(PresentationViewModel requirementVM, List<string> mailReceipts);
        Task SendRequirementPresentationRejectNotificationMessageAsync(PresentationViewModel requirementVM, List<string> mailReceipts);

        Task SendUserRegistrationMessageAsync(UserManager<ApplicationUser> userManager, string userId, string uriScheme, UrlHelper url);
        Task SendUserRegistrationMessageAsync(UserManager<ContractorUser> userManager, string userId, string uriScheme, UrlHelper url);
        Task SendUserRegistrationMessageAsync(UserManager<CustomerUser> userManager, string userId, string uriScheme, UrlHelper url);

        Task SendUserResetPasswordMessageAsync(UserManager<ApplicationUser> userManager, string userId, string uriScheme, UrlHelper url);
    }
}
