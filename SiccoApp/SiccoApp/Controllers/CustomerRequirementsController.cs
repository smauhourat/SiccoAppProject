using SiccoApp.Models;
using SiccoApp.Persistence;
using SiccoApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;

namespace SiccoApp.Controllers
{
    public class CustomerRequirementsController : BaseController
    {
        private IContractorRepository contractorRepository = null;
        private IContractRepository contractRepository = null;
        private IPeriodRepository periodRepository = null;
        private IRequirementRepository requirementRepository = null;
        private IPresentationRepository presentationRepository = null;
        private IPresentationServices presentationServices = null;
        private IDocumentFileService documentFileService = null;
        private IWorkflowMessageService workflowMessageService = null;

        public CustomerRequirementsController(IContractorRepository contractorRepo,
                                            IContractRepository contractRepo,
                                            IPeriodRepository periodRepo,
                                            IRequirementRepository requirementRepo,
                                            IPresentationRepository presentationRepo,
                                            IPresentationServices presentationServ,
                                            IDocumentFileService documentFileServ,
                                            IWorkflowMessageService workflowMessageServ)
        {
            contractorRepository = contractorRepo;
            contractRepository = contractRepo;
            periodRepository = periodRepo;
            requirementRepository = requirementRepo;
            presentationRepository = presentationRepo;
            presentationServices = presentationServ;
            documentFileService = documentFileServ;
            workflowMessageService = workflowMessageServ;
        }

        // GET: CustomerRequirements
        public async Task<ActionResult> Index(int? contractorID, int? contractID, int? periodID)
        {
            //Contractors por Customer
            ViewBag.ContractorID = new SelectList(await contractorRepository.FindContractorsAsync(base.CurrentCustomerID), "ContractorID", "CompanyName");

            //Contracts por Contractor
            ViewBag.ContractID = new SelectList(await contractRepository.FindContractsAsync(base.CurrentContractorID), "ContractID", "ContractCode");

            ViewBag.PeriodID = new SelectList(await periodRepository.FindPeriodsAsync(), "PeriodID", "PeriodID");

            RequirementStatus requirementStatus = 0; // !string.IsNullOrEmpty(string.Empty) ? (RequirementStatus)Enum.Parse(typeof(RequirementStatus), requirementStatusID) : 0;
            var requirements = await requirementRepository.FindRequirementsByFilterAsync(0, contractorID == null ? 0 : contractorID.Value, contractID == null ? 0 : contractID.Value, periodID == null ? 0 : periodID.Value, requirementStatus, 0);

            var requirementsViewModel = new RequirementListViewModel(requirements.ToList());
            return View(requirementsViewModel);
        }

        public async Task<JsonResult> GetContractsByContractor(int contractorID)
        {
            return Json(new SelectList(await contractRepository.FindContractsAsync(base.CurrentContractorID), "ContractID", "ContractCode"), JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (contractorRepository != null)
                {
                    contractorRepository.Dispose();
                    contractorRepository = null;
                }
                
                if (contractRepository != null)
                {
                    contractRepository.Dispose();
                    contractRepository = null;
                }
                
                if (periodRepository != null)
                {
                    periodRepository.Dispose();
                    periodRepository = null;
                }
                
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
                
                if (presentationServices != null)
                {
                    presentationServices.Dispose();
                    presentationServices = null;
                }
                
                //documentFileService.Dispose();
                if (workflowMessageService != null)
                {
                    workflowMessageService.Dispose();
                    workflowMessageService = null;
                }                
            }
            base.Dispose(disposing);
        }
    }
}