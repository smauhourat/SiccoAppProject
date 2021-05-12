using SiccoApp.Models;
using SiccoApp.Persistence;
using SiccoApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SiccoApp.Helpers;

namespace SiccoApp.Controllers
{
    public class ContractorRequirementsController : BaseController 
    {
        private IContractorRepository contractorRepository = null;
        private IContractRepository contractRepository = null;
        private IPeriodRepository periodRepository = null;
        private IRequirementRepository requirementRepository = null;
        private IPresentationRepository presentationRepository = null;
        private IPresentationServices presentationServices = null;
        private IDocumentFileService documentFileService = null;
        private IWorkflowMessageService workflowMessageService = null;
        private IEntityTypeRepository entityTypeRepository = null;

        public ContractorRequirementsController(IContractorRepository contractorRepo,
                                            IContractRepository contractRepo,
                                            IPeriodRepository periodRepo,
                                            IRequirementRepository requirementRepo, 
                                            IPresentationRepository presentationRepo,
                                            IPresentationServices presentationServ,
                                            IDocumentFileService documentFileServ,
                                            IWorkflowMessageService workflowMessageServ,
                                            IEntityTypeRepository entityTypeRepo)
        {
            contractorRepository = contractorRepo;
            contractRepository = contractRepo;
            periodRepository = periodRepo;
            requirementRepository = requirementRepo;
            presentationRepository = presentationRepo;
            presentationServices = presentationServ;
            documentFileService = documentFileServ;
            workflowMessageService = workflowMessageServ;
            entityTypeRepository = entityTypeRepo;
        }

        [AuthorizeRoles(RoleNames.ContractorRole)]
        public async Task<ActionResult> Index(int? contractID, int? periodID, string requirementStatusID, int? entityTypeID)
        {
            ViewBag.ContractID = new SelectList(await contractRepository.FindContractsAsync(base.CurrentContractorID), "ContractID", "ContractCode");
            ViewBag.PeriodID = new SelectList(await periodRepository.FindPeriodsAsync(), "PeriodID", "PeriodID");
            ViewBag.RequirementStatusID = new SelectList(Enum.GetNames(typeof(RequirementStatus)));
            ViewBag.EntityTypeID = new SelectList(await entityTypeRepository.EntityTypesAsync(), "EntityTypeID", "Description");

            RequirementStatus requirementStatus = !string.IsNullOrEmpty(requirementStatusID) ? (RequirementStatus)Enum.Parse(typeof(RequirementStatus), requirementStatusID) : 0;

            var requirements = await requirementRepository.FindRequirementsByFilterAsync(0, 0, contractID == null ? 0 : contractID.Value, periodID == null ? 0 : periodID.Value, requirementStatus, entityTypeID == null ? 0 : entityTypeID.Value);

            var requirementsViewModel = new RequirementListViewModel(requirements.ToList());
            return View(requirementsViewModel);
        }

        // GET: ContractorRequirements/PresentationAttach
        [AuthorizeRoles(RoleNames.ContractorRole)]
        public async Task<ActionResult> PresentationAttach(int requirementID)
        {
            Requirement requirement = await requirementRepository.FindRequirementByIDAsync(requirementID);
            PresentationViewModel model = new PresentationViewModel(requirement);
            return View(model);
        }

        [HttpPost]
        [AuthorizeRoles(RoleNames.ContractorRole)]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> PresentationAttach(PresentationViewModel model, HttpPostedFileBase documentFiles)
        {

            if (ModelState.IsValid)
            {
                Presentation presentation = (Presentation)model.GetPresentation();

                var requirement = await requirementRepository.FindRequirementByIDAsync(model.RequirementID);
                presentation.RequirementID = model.RequirementID;
               
                try
                {
                    //Se suben los documentos
                    presentation.DocumentFiles = await documentFileService.UploadDocumentFileAsync(documentFiles, DocumentManager.GetFileName(requirement));

                    //Se crea la Presentacion
                    await presentationServices.CreateAsync(presentation);

                    //Se recuperan los mails de los auditores
                    var mailReceipts = await presentationServices.GetMailsAuditors(presentation);

                    //Se envian el mail avisando que se subio una Presentacion a un Requerimiento
                    //workflowMessageService.SendRequirementPresentationNotificationMessage(new PresentationViewModel(presentation), mailReceipts);
                    await workflowMessageService.SendRequirementPresentationNotificationMessageAsync(presentation.ToDisplayViewModel(), mailReceipts);

                    return RedirectToAction("Index", Request.QueryString.ToRouteValues());
                }
                catch (Exception e)
                {
                    var errors = string.Join(",", e.Message);
                    ModelState.AddModelError(string.Empty, errors);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // GET: ContractorRequirements/PresentationsIndex
        [AuthorizeRoles(RoleNames.ContractorRole)]
        public async Task<ActionResult> PresentationsIndex(int requirementID)
        {
            var presentations = await requirementRepository.FindPresentationsAsync(requirementID);
            var presentationsListViewModel = new PresentationListViewModel(presentations);

            return View(presentationsListViewModel);
        }
        // GET: ContractorRequirements/PresentationDelete/5
        public async Task<ActionResult> PresentationDelete(int? presentationID)
        {
            if (presentationID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Presentation presentation = await presentationRepository.FindByIdAsync(presentationID.Value);
            if (presentation == null)
            {
                return HttpNotFound();
            }
            var presentationViewModel = new PresentationViewModel(presentation);
            return View(presentationViewModel);
        }

        // POST: ContractorRequirements/PresentationDelete/5
        [HttpPost, ActionName("PresentationDelete")]
        [AuthorizeRoles(RoleNames.ContractorRole)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> PresentationDeleteConfirmed(int presentationID)
        {
            //await presentationRepository.DeleteAsync(presentationID);
            //return RedirectToAction("Index");
            try
            {
                //Se recuperan los mails de los auditores y se mantiene los datos de la Presentacion
                Presentation presentation = await presentationRepository.FindByIdAsync(presentationID);
                var mailReceipts = await presentationServices.GetMailsAuditors(presentation);
                var model = new PresentationViewModel(presentation);

                //Se elimina la Presentacion
                await presentationRepository.DeleteAsync(presentationID);

                //Se envian el mail avisando que se elimino una Presentacion
                await workflowMessageService.SendRequirementPresentationDeleteNotificationMessageAsync(model, mailReceipts);

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                var errors = string.Join(",", e.Message);
                ModelState.AddModelError(string.Empty, errors);
            }

            return View();
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

                if (entityTypeRepository != null)
                {
                    entityTypeRepository.Dispose();
                    entityTypeRepository = null;
                }
            }
            base.Dispose(disposing);
        }
    }
}