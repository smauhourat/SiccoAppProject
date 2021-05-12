using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SiccoApp.Persistence;
using SiccoApp.Models;
using SiccoApp.Services;
using SiccoApp.Helpers;

namespace SiccoApp.Controllers
{
    public class RequirementsController : BaseController
    {
        private IRequirementRepository requirementRepository = null;
        private IPresentationRepository presentationRepository = null;
        private IDocumentFileService documentFileService = null;
        private ICustomerAuditorRespository customerAuditorRespository = null;
        private IContractorRepository contractorRepository = null;
        private IPresentationServices presentationServices = null;
        private IWorkflowMessageService workflowMessageService = null;
        private IDocumentationBusinessTypeRepository documentationBusinessTypeRepository = null;
        private IPeriodRepository periodRepository = null;
        private IEntityTypeRepository entityTypeRepository = null;

        public RequirementsController(IRequirementRepository requirementRepo, 
                                    IPresentationRepository presentationRepo, 
                                    IDocumentFileService documentFileServ, 
                                    ICustomerAuditorRespository customerAuditorRespo, 
                                    IContractorRepository contractorRepo, 
                                    IPresentationServices presentationServ, 
                                    IWorkflowMessageService workflowMessageServ,
                                    IDocumentationBusinessTypeRepository documentationBusinessTypeRepo,
                                    IPeriodRepository periodRepo,
                                    IEntityTypeRepository entityTypeRepo)
        {
            requirementRepository = requirementRepo;
            presentationRepository = presentationRepo;
            documentFileService = documentFileServ;
            customerAuditorRespository = customerAuditorRespo;
            contractorRepository = contractorRepo;
            presentationServices = presentationServ;
            workflowMessageService = workflowMessageServ;
            documentationBusinessTypeRepository = documentationBusinessTypeRepo;
            periodRepository = periodRepo;
            entityTypeRepository = entityTypeRepo;
        }

        // GET: Requirements
        [Authorize(Roles = "AdminRole,CustomerRole,CustomerAdminRole, CustomerAuditorRole")]
        public async Task<ActionResult> Index(int? customerID, int? contractorID, int? periodID, string requirementStatusID, int? entityTypeID)
        {
            var customersByAuditor = await customerAuditorRespository.FindCustomerAuditorsByUserIDAsync(this.CurrentUserID);
            var customerIDFilter = customerID == null ? customersByAuditor.FirstOrDefault().CustomerID : customerID;

            var contractors = await contractorRepository.FindContractorsByCustomerIDAsync((int)customerIDFilter);
            var contractorIDFilter = contractors != null && contractorID == null ? contractors.FirstOrDefault()?.ContractorID : contractorID;

            ViewBag.PeriodID = new SelectList(await periodRepository.FindPeriodsAsync(), "PeriodID", "PeriodID");
            ViewBag.RequirementStatusID = new SelectList(Enum.GetNames(typeof(RequirementStatus)));
            ViewBag.EntityTypeID = new SelectList(await entityTypeRepository.EntityTypesAsync(), "EntityTypeID", "Description");

            RequirementStatus requirementStatus = !string.IsNullOrEmpty(requirementStatusID) ? (RequirementStatus)Enum.Parse(typeof(RequirementStatus), requirementStatusID) : 0;

            //var requirements = await requirementRepository.FindRequirementsByFilterAsync((int)customerIDFilter, (int)contractorIDFilter, 0, periodID == null ? 0 : periodID.Value, requirementStatus, entityTypeID == null ? 0 : entityTypeID.Value);
            var requirements = await requirementRepository.FindRequirementsByFilterAsync((int)customerIDFilter, contractorIDFilter == null ? 0 : contractorIDFilter.Value, 0, periodID == null ? 0 : periodID.Value, requirementStatus, entityTypeID == null ? 0 : entityTypeID.Value);


            var requirementsViewModel = new RequirementListViewModel(requirements.ToList());

            ViewBag.CustomerID = customerIDFilter;
            ViewBag.ContractorId = contractorIDFilter;

            requirementsViewModel.Customers = customersByAuditor.Select(x => new SelectListItem() { Value = x.CustomerID.ToString(), Text = x.Customer.FullCompanyName });
            requirementsViewModel.Contractors = contractors.Select(x => new SelectListItem() { Value = x.ContractorID.ToString(), Text = x.FullCompanyName });


            return View(requirementsViewModel);
        }

        [HttpGet]
        public ActionResult GetContractors(int customerID)
        {

            if (customerID > 0)
            {
                var contractors = contractorRepository.FindContractorsByCustomerID(customerID);
                IEnumerable<SelectListItem> list = new SelectList(contractors, "ContractorID", "FullCompanyName");

                return Json(list, JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        // GET: Requirements/Details/5
        [Authorize(Roles = "AdminRole,CustomerRole,CustomerAdminRole, CustomerAuditorRole")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //Requirement requirement = await db.Requirements.FindAsync(id);
            Requirement requirement = await requirementRepository.FindRequirementByIDAsync((int)id);
            if (requirement == null)
            {
                return HttpNotFound();
            }
            return View(requirement);
        }

        // GET: Requirements/Create
        [Authorize(Roles = "AdminRole,CustomerRole,CustomerAdminRole, CustomerAuditorRole")]
        public ActionResult Create()
        {
            //ViewBag.DocumentationBusinessTypeID = new SelectList(db.DocumentationBusinessTypes, "DocumentationBusinessTypeID", "DocumentationBusinessTypeID");
            ViewBag.DocumentationBusinessTypeID = new SelectList(documentationBusinessTypeRepository.DocumentationBusinessTypes(), "DocumentationBusinessTypeID", "DocumentationBusinessTypeID");
            return View();
        }

        // POST: Requirements/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "AdminRole,CustomerRole,CustomerAdminRole, CustomerAuditorRole")]
        public async Task<ActionResult> Create([Bind(Include = "RequirementID,DocumentationBusinessTypeID,ContractID,EmployeeContractID,VehicleContractID,PeriodID,RequirementStatus,DueDate")] Requirement requirement)
        {
            if (ModelState.IsValid)
            {
                await requirementRepository.CreateAsync(requirement);

                return RedirectToAction("Index");
            }

            //ViewBag.DocumentationBusinessTypeID = new SelectList(db.DocumentationBusinessTypes, "DocumentationBusinessTypeID", "DocumentationBusinessTypeID", requirement.DocumentationBusinessTypeID);
            ViewBag.DocumentationBusinessTypeID = new SelectList(documentationBusinessTypeRepository.DocumentationBusinessTypes(), "DocumentationBusinessTypeID", "DocumentationBusinessTypeID", requirement.DocumentationBusinessTypeID);
            return View(requirement);
        }

        // GET: Requirements/Edit/5
        [Authorize(Roles = "AdminRole,CustomerRole,CustomerAdminRole, CustomerAuditorRole")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            Requirement requirement = await requirementRepository.FindRequirementByIDAsync((int)id);
            if (requirement == null)
            {
                return HttpNotFound();
            }

            ViewBag.DocumentationBusinessTypeID = new SelectList(documentationBusinessTypeRepository.DocumentationBusinessTypes(), "DocumentationBusinessTypeID", "DocumentationBusinessTypeID", requirement.DocumentationBusinessTypeID);
            return View(requirement);
        }

        // POST: Requirements/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "AdminRole,CustomerRole,CustomerAdminRole, CustomerAuditorRole")]
        public async Task<ActionResult> Edit([Bind(Include = "RequirementID,DocumentationBusinessTypeID,ContractID,EmployeeContractID,VehicleContractID,PeriodID,RequirementStatus,DueDate")] Requirement requirement)
        {
            if (ModelState.IsValid)
            {
                await requirementRepository.UpdateAsync(requirement);

                return RedirectToAction("Index");
            }
            ViewBag.DocumentationBusinessTypeID = new SelectList(documentationBusinessTypeRepository.DocumentationBusinessTypes(), "DocumentationBusinessTypeID", "DocumentationBusinessTypeID", requirement.DocumentationBusinessTypeID);
            return View(requirement);
        }

        // GET: Requirements/Delete/5
        [Authorize(Roles = "AdminRole,CustomerRole,CustomerAdminRole, CustomerAuditorRole")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Requirement requirement = await requirementRepository.FindRequirementByIDAsync((int)id);
            if (requirement == null)
            {
                return HttpNotFound();
            }
            return View(requirement);
        }

        // POST: Requirements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "AdminRole,CustomerRole,CustomerAdminRole, CustomerAuditorRole")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await requirementRepository.DeleteAsync(id);

            return RedirectToAction("Index");
        }

        // GET: Requirements/PresentationAttach
        [Authorize(Roles = "AdminRole,CustomerRole,CustomerAdminRole, CustomerAuditorRole")]
        public async Task<ActionResult> PresentationAttach(int requirementID, int customerID, int contractorID)
        {
            //PresentationViewModel model = new PresentationViewModel();
            //model.RequirementID = requirementID;
            //return View(model);
            Requirement requirement = await requirementRepository.FindRequirementByIDAsync(requirementID);
            ViewBag.CustomerID = customerID;
            ViewBag.ContractorId = contractorID;
            PresentationViewModel model = new PresentationViewModel(requirement);
            return View(model);

        }

        [HttpPost]
        //[Authorize(Roles = "AdminRole")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "AdminRole,CustomerRole,CustomerAdminRole, CustomerAuditorRole")]
        public async Task<ActionResult> PresentationAttach(PresentationViewModel model, HttpPostedFileBase documentFiles, int customerID, int contractorID)
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
                    //var mailReceipts = await presentationServices.GetMailsAuditors(presentation);

                    //Se envian el mail avisando que se subio un requerimiento
                    //workflowMessageService.SendRequirementPresentationNotificationMessage(new PresentationViewModel(presentation), mailReceipts);
                    //workflowMessageService.SendRequirementPresentationNotificationMessage(presentation.ToDisplayViewModel(), mailReceipts);

                    return RedirectToAction("PresentationsIndex", "Requirements", new { requirementID = model.RequirementID, customerID = customerID, contractorID = contractorID });
                    //return RedirectToAction("Index", Request.QueryString.ToRouteValues());
                }
                catch (Exception e)
                {
                    var errors = string.Join(",", e.Message);
                    ModelState.AddModelError(string.Empty, errors);
                }
            }

            return View(model);
        }

        // GET: Requirements/PresentationsIndex
        [Authorize(Roles = "AdminRole,CustomerRole,CustomerAdminRole, CustomerAuditorRole")]
        public async Task<ActionResult> PresentationsIndex(int requirementID, int customerID, int contractorID)
        {
            var presentations = await requirementRepository.FindPresentationsAsync(requirementID);
            var presentationsListViewModel = new PresentationListViewModel(presentations);
            ViewBag.CustomerID = customerID;
            ViewBag.ContractorId = contractorID;

            return View(presentationsListViewModel);
        }

        // GET: Requirements/PresentationDelete/5
        [Authorize(Roles = "AdminRole,CustomerRole,CustomerAdminRole, CustomerAuditorRole")]
        public async Task<ActionResult> PresentationDelete(int? presentationID, int requirementID, int customerID, int contractorID)
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
            ViewBag.CustomerID = customerID;
            ViewBag.ContractorId = contractorID;

            var presentationViewModel = new PresentationViewModel(presentation);
            return View(presentationViewModel);
        }

        // POST: Requirements/PresentationDelete/5
        [HttpPost, ActionName("PresentationDelete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "AdminRole,CustomerRole,CustomerAdminRole, CustomerAuditorRole")]
        public async Task<ActionResult> PresentationDeleteConfirmed(int presentationID, int requirementID, int customerID, int contractorID)
        {
            await presentationRepository.DeleteAsync(presentationID);

            return RedirectToAction("Index", "Requirements", new { customerID = customerID, contractorID = contractorID });
            //return RedirectToAction("PresentationsIndex", "Requirements", new { requirementID = requirementID, customerID = customerID, contractorID = contractorID });
            //return RedirectToAction("Index");
        }

        [Authorize(Roles = "AdminRole,CustomerRole,CustomerAdminRole, CustomerAuditorRole")]
        public async Task<ActionResult> PresentationTake(int? presentationID, int customerID, int contractorID)
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
            presentation.TakenForID = this.CurrentUserID;
            await presentationRepository.TakeToAudit(presentation);

            return RedirectToAction("PresentationsIndex", "Requirements", new { requirementID = presentation.RequirementID, customerID = customerID, contractorID = contractorID });
            //return RedirectToAction("Index");
        }

        // GET: Requirements/PresentationApprove/5
        [Authorize(Roles = "AdminRole,CustomerRole,CustomerAdminRole, CustomerAuditorRole")]
        public async Task<ActionResult> PresentationApprove(int? presentationID, int customerID, int contractorID)
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
            ViewBag.CustomerID = customerID;
            ViewBag.ContractorId = contractorID;

            var presentationViewModel = new PresentationViewModel(presentation);
            return View(presentationViewModel);
        }

        [HttpPost, ActionName("PresentationApprove")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "AdminRole,CustomerRole,CustomerAdminRole, CustomerAuditorRole")]
        public async Task<ActionResult> PresentationApproveConfirmed(PresentationViewModel model, int customerID, int contractorID)
        {
            if (ModelState.IsValid)
            {
                Presentation presentation = await presentationRepository.FindByIdAsync(model.PresentationID);
                presentation.Observations = model.Observations;
                presentation.ApprovedForID = this.CurrentUserID;
                await presentationRepository.Approve(presentation);

                //Se recuperan los mails de los usuarios del Contratista
                var mailReceipts = await presentationServices.GetMailsContractors(contractorID);

                //Se envia un mail Avisando la situacion
                await workflowMessageService.SendRequirementPresentationApproveNotificationMessageAsync(presentation.ToDisplayViewModel(), mailReceipts);

            }

            return RedirectToAction("PresentationsIndex", "Requirements", new { requirementID = model.RequirementID, customerID = customerID, contractorID = contractorID });

            //return RedirectToAction("Index");
        }

        // GET: Requirements/PresentationReject/5
        [Authorize(Roles = "AdminRole,CustomerRole,CustomerAdminRole, CustomerAuditorRole")]
        public async Task<ActionResult> PresentationReject(int? presentationID, int customerID, int contractorID)
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
            ViewBag.CustomerID = customerID;
            ViewBag.ContractorId = contractorID;

            var presentationViewModel = new PresentationViewModel(presentation);
            return View(presentationViewModel);
        }

        [HttpPost, ActionName("PresentationReject")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "AdminRole,CustomerRole,CustomerAdminRole, CustomerAuditorRole")]
        public async Task<ActionResult> PresentationRejectConfirmed(PresentationViewModel model, int customerID, int contractorID)
        {
            if (ModelState.IsValid)
            {
                Presentation presentation = await presentationRepository.FindByIdAsync(model.PresentationID);
                presentation.Observations = model.Observations;
                presentation.RejectedForID = this.CurrentUserID;
                await presentationRepository.Reject(presentation);

                //Se recuperan los mails de los usuarios del Contratista
                var mailReceipts = await presentationServices.GetMailsContractors(contractorID);

                //Se envia un mail Avisando la situacion
                await workflowMessageService.SendRequirementPresentationRejectNotificationMessageAsync(presentation.ToDisplayViewModel(), mailReceipts);
            }

            return RedirectToAction("PresentationsIndex", "Requirements", new { requirementID = model.RequirementID, customerID = customerID, contractorID = contractorID });
            //return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {                
                if (presentationRepository != null)
                {
                    presentationRepository.Dispose();
                    presentationRepository = null;
                }
                
                //documentFileService.Dispose();
                if (requirementRepository != null)
                {
                    requirementRepository.Dispose();
                    requirementRepository = null;
                }
                
                if (customerAuditorRespository != null)
                {
                    customerAuditorRespository.Dispose();
                    customerAuditorRespository = null;
                }
                
                if (contractorRepository != null)
                {
                    contractorRepository.Dispose();
                    contractorRepository = null;
                }
                
                if (presentationServices != null)
                {
                    presentationServices.Dispose();
                    presentationServices = null;
                }
                
                if (workflowMessageService != null)
                {
                    workflowMessageService.Dispose();
                    workflowMessageService = null;
                }

                if (documentationBusinessTypeRepository != null)
                {
                    documentationBusinessTypeRepository.Dispose();
                    documentationBusinessTypeRepository = null;
                }

                if (periodRepository != null)
                {
                    periodRepository.Dispose();
                    periodRepository = null;
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
