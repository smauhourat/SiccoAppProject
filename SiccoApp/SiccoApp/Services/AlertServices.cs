using SiccoApp.Messaging;
using SiccoApp.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;

namespace SiccoApp.Services
{
    public class AlertServices : IAlertServices
    {
        private IRequirementRepository requirementRepository = null;
        private IPresentationRepository presentationRepository = null;
        private IEmailManager emailManager;
        private IEmailAccountRepository emailAccountRepository = null;
        private IContractorRepository contractorRepository = null;

        public AlertServices(IRequirementRepository requirementRepo, 
            IPresentationRepository presentationRepo, 
            IEmailManager emailMan, 
            IEmailAccountRepository emailAccountRepo, 
            IContractorRepository contractorRepo)
        {
            requirementRepository = requirementRepo;
            presentationRepository = presentationRepo;
            emailManager = emailMan;
            emailAccountRepository = emailAccountRepo;
            contractorRepository = contractorRepo;
        }

        /// <summary>
        /// Send Mails to Contractors, with list of Expired Requirements
        /// </summary>
        /// <returns></returns>
        public async Task SendMailDueDateRequirements()
        {
            EmailAccount emailAccount = emailAccountRepository.FindEmailAccountsDefaultAsync();

            var list = await requirementRepository.FindRequirementsNextToExpireAsync();
            int i = 1;
            foreach (Requirement item in list)
            {
                if (i == 3)
                    break;
                emailManager.SendSimpleMessage(emailAccount, emailAccount.Email, "santiagomauhourat@hotmail.com", "Santiago", "Prueba", item.RequirementID + " - " + item.DueDate);
                i++;
            }
        }

        private string BuildPresentationsList(IList<Presentation> list, int contractorID)
        {
            var filteredList = list.Where(x => x.Requirement.Contract.Contractor.ContractorID == contractorID).ToList();
            string result = "";

            foreach (var item in filteredList)
            {
                result += item.Requirement.DocumentationBusinessType.Documentation.DocumentationCode + " - " + item.Requirement.DocumentationBusinessType.Documentation.Description + " - " + item.Requirement.RequirementStatus;
                result += Environment.NewLine;
            }

            return result;
        }

        /// <summary>
        /// Send Mails to Contractors, with list of Processed Presentations By Period
        /// </summary>
        /// <returns></returns>
        public async Task SendMailProcessedPresentations()
        {
            EmailAccount emailAccount = emailAccountRepository.FindEmailAccountsDefaultAsync();
            
            int periodID = int.Parse(DateTime.Now.Year.ToString() + (DateTime.Now.Month < 10 ? "0" + DateTime.Now.Month : DateTime.Now.Month.ToString()));

            //All Presentations for Period
            var presentations = await presentationRepository.FindByPeriodAsync(periodID);

            //All distinct Contractors for Presentations
            var contractors = presentations.GroupBy(c => c.Requirement.Contract.ContractID)
                .Select(x => x.First())
                .ToList();

            foreach (var item in contractors)
            {
                var contractor = await contractorRepository.FindContractorsByIDAsync(item.Requirement.Contract.ContractorID);
                foreach (var user in contractor.Users)
                {
                    var body = BuildPresentationsList(presentations, item.Requirement.Contract.ContractorID);
                    //emailManager.SendSimpleMessage(emailAccount, emailAccount.Email, user.Email, user.FirstName + " " + user.LastName, "Documentacion Auditada en el Periodo: " + periodID.ToString(), item.PresentationID.ToString());
                    emailManager.SendSimpleMessage(emailAccount, emailAccount.Email, "santiagomauhourat@hotmail.com", user.FirstName + " " + user.LastName, user.Email + " Documentacion Auditada en el Periodo: " + periodID.ToString(), item.PresentationID.ToString());
                }
            }
        }


        #region IDisposable Support
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
                if (requirementRepository != null)
                {
                    requirementRepository.Dispose();
                    requirementRepository = null;
                }
                
                if (presentationRepository != null)
                {
                    presentationRepository.Dispose();
                    presentationRepository = null;
                }
                
                if (emailAccountRepository != null)
                {
                    emailAccountRepository.Dispose();
                    emailAccountRepository = null;
                }

                if (contractorRepository != null)
                {
                    contractorRepository.Dispose();
                    contractorRepository = null;
                }
            }
        }
        #endregion
    }
}