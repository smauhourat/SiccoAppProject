using SiccoApp.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;

namespace SiccoApp.Services
{
    public class PresentationServices : IPresentationServices
    {
        private IPresentationRepository presentationRepository = null;
        private ICustomerAuditorRespository customerAuditorRespository = null;
        private IContractorRepository contractorRepository = null;
        private IWorkflowMessageService workflowMessageService = null;

        public PresentationServices(IPresentationRepository presentationRepo, 
            ICustomerAuditorRespository customerAuditorRespo, 
            IWorkflowMessageService workflowMessageServ,
            IContractorRepository contractorRepo)
        {
            presentationRepository = presentationRepo;
            customerAuditorRespository = customerAuditorRespo;
            workflowMessageService = workflowMessageServ;
            contractorRepository = contractorRepo;
        }

        public async Task CreateAsync(Presentation presentationToAdd)
        {
            //se crea la presentacion de un Requerimiento por parte de un Contratista para el Cliente
            await presentationRepository.CreateAsync(presentationToAdd);
        }

        public async Task<List<String>> GetMailsAuditors(Presentation presentation)
        {
            //se recuperan los mails de los auditores de Cliente
            var auditors = await customerAuditorRespository.FindCustomerAuditorsByCustomerAsync(presentation.Requirement.Contract.CustomerID);
            var mailReceipts = new List<string>();
            foreach (var auditor in auditors)
            {
                //Ojoooo
                mailReceipts.Add(auditor.User.Email);
            }
            return mailReceipts;
        }

        public async Task<List<String>> GetMailsContractors(int contractorID)
        {
            //se recuperan los mails de los usuarios del Contratista
            var contractor = await contractorRepository.FindContractorsByIDAsync(contractorID);
            return contractor.Users.ToList<ContractorUser>().Select(m => m.Email).ToList();
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
                presentationRepository.Dispose();
                customerAuditorRespository.Dispose();
                workflowMessageService.Dispose();
                contractorRepository.Dispose();
            }
        }
    }
}