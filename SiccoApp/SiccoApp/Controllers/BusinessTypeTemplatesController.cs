using System;
using System.Linq;
using System.Web.Mvc;
using SiccoApp.Persistence;
using System.Threading.Tasks;
using SiccoApp.Models;

namespace SiccoApp.Controllers
{
    public class BusinessTypeTemplatesController : BaseController
    {
        private IBusinessTypeTemplateRepository businessTypeTemplateRepository = null;
        private IDocumentationBusinessTypeTemplateRepository documentationBusinessTypeTemplateRepository = null;
        private IDocumentationTemplateRepository documentationTemplateRepository = null;
        //private IDocumentationImportanceRepository documentationImportanceRepository = null;
        private IDocumentationPeriodicityRepository documentationPeriodicityRepository = null;
        //private IRepository<DocumentationImportance> documentationImportanceRepository;

        public BusinessTypeTemplatesController(
            IBusinessTypeTemplateRepository businessTypeTemplateRepo, 
            IDocumentationBusinessTypeTemplateRepository documentationBusinessTypeTemplateRepo, 
            IDocumentationTemplateRepository documentationTemplateRepo, 
            //IDocumentationImportanceRepository documentationImportanceRepo, 
            IDocumentationPeriodicityRepository documentationPeriodicityRepo)
        {
            businessTypeTemplateRepository = businessTypeTemplateRepo;
            documentationBusinessTypeTemplateRepository = documentationBusinessTypeTemplateRepo;
            documentationTemplateRepository = documentationTemplateRepo;
            //documentationImportanceRepository = documentationImportanceRepo;
            documentationPeriodicityRepository = documentationPeriodicityRepo;
        }

        // GET: BusinessTypeTemplates
        [AuthorizeRoles(RoleNames.AdminRole)]
        public async Task<ActionResult> Index()
        {
            var businessTypeTemplates = await businessTypeTemplateRepository.FindBusinessTypeTemplatesAsync();

            return View(businessTypeTemplates.ToList());
        }

        // GET: BusinessTypeTemplates/Details/5
        [AuthorizeRoles(RoleNames.AdminRole)]
        public async Task<ActionResult> Details(int id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            BusinessTypeTemplate businessTypeTemplate = await businessTypeTemplateRepository.FindBusinessTypeTemplateByIDAsync(id);
            if (businessTypeTemplate == null)
            {
                return HttpNotFound();
            }
            return View(businessTypeTemplate);
        }

        // GET: BusinessTypeTemplates/Create
        [AuthorizeRoles(RoleNames.AdminRole)]
        public ActionResult Create()
        {
            return View();
        }

        // POST: BusinessTypeTemplates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [AuthorizeRoles(RoleNames.AdminRole)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "BusinessTypeTemplateID,BusinessTypeTemplateCode,Description")] BusinessTypeTemplate businessTypeTemplate)
        {
            if (ModelState.IsValid)
            {
                await businessTypeTemplateRepository.CreateAsync(businessTypeTemplate);
                return RedirectToAction("Index");
            }

            return View(businessTypeTemplate);
        }

        // GET: BusinessTypeTemplates/Edit/5
        [AuthorizeRoles(RoleNames.AdminRole)]
        public async Task<ActionResult> Edit(int id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            BusinessTypeTemplate businessTypeTemplate = await businessTypeTemplateRepository.FindBusinessTypeTemplateByIDAsync(id);
            if (businessTypeTemplate == null)
            {
                return HttpNotFound();
            }
            return View(businessTypeTemplate);
        }

        // POST: BusinessTypeTemplates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [AuthorizeRoles(RoleNames.AdminRole)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "BusinessTypeTemplateID,BusinessTypeTemplateCode,Description")] BusinessTypeTemplate businessTypeTemplate)
        {
            if (ModelState.IsValid)
            {
                await businessTypeTemplateRepository.UpdateAsync(businessTypeTemplate);

                return RedirectToAction("Index");
            }
            return View(businessTypeTemplate);
        }

        // GET: BusinessTypeTemplates/Delete/5
        [AuthorizeRoles(RoleNames.AdminRole)]
        public async Task<ActionResult> Delete(int id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            BusinessTypeTemplate businessTypeTemplate = await businessTypeTemplateRepository.FindBusinessTypeTemplateByIDAsync(id);
            if (businessTypeTemplate == null)
            {
                return HttpNotFound();
            }
            return View(businessTypeTemplate);
        }

        // POST: BusinessTypeTemplates/Delete/5
        [HttpPost, ActionName("Delete")]
        [AuthorizeRoles(RoleNames.AdminRole)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await businessTypeTemplateRepository.DeleteAsync(id);
            return RedirectToAction("Index");
        }

        // GET: BusinessTypeTemplates/AssignedDocuments/5
        [AuthorizeRoles(RoleNames.AdminRole)]
        public async Task<ActionResult> AssignedDocuments(int id)
        {
            var documentationBusinessTypeTemplates = await documentationBusinessTypeTemplateRepository.FindDocumentationBusinessTypeTemplatesByBTAsync(id);

            BusinessTypeTemplate businessTypeTemplate = await businessTypeTemplateRepository.FindBusinessTypeTemplateByIDAsync(id);
            if (businessTypeTemplate == null)
            {
                return HttpNotFound();
            }

            var viewModel = new AssignedDocumentTemplatesViewModel(businessTypeTemplate, documentationBusinessTypeTemplates);

            return View(viewModel);

        }

        // GET: BusinessTypeTemplates/DocumentsCreate
        [AuthorizeRoles(RoleNames.AdminRole)]
        public async Task<ActionResult> DocumentsCreate(int businessTypeTemplateID)
        {
            EditAssignDocumentTemplateViewModel model = new EditAssignDocumentTemplateViewModel();
            model.BusinessTypeTemplateID = businessTypeTemplateID;

            BusinessTypeTemplate businessTypeTemplate = await businessTypeTemplateRepository.FindBusinessTypeTemplateByIDAsync(businessTypeTemplateID);
            if (businessTypeTemplate != null)
            {
                ViewBag.BussinessTypeTemplateDescription = businessTypeTemplate.Description;
            }

            ViewBag.DocumentationTemplateID = new SelectList(documentationTemplateRepository.UnAssignedDocumentationTemplates(businessTypeTemplateID), "DocumentationTemplateID", "DocumentationTemplateCode");
            //ViewBag.DocumentationImportanceID = new SelectList(documentationImportanceRepository.DocumentationImportances(), "DocumentationImportanceID", "Description");
            ViewBag.DocumentationPeriodicityID = new SelectList(documentationPeriodicityRepository.DocumentationPeriodicitys(), "DocumentationPeriodicityID", "Description");

            return View(model);
        }

        [HttpPost]
        [AuthorizeRoles(RoleNames.AdminRole)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DocumentsCreate(EditAssignDocumentTemplateViewModel model)
        {

            if (ModelState.IsValid)
            {
                DocumentationBusinessTypeTemplate document = (DocumentationBusinessTypeTemplate)model.GetDocument();
                //document.BusinessTypeTemplateID = model.BusinessTypeTemplateID;

                try
                {
                    await documentationBusinessTypeTemplateRepository.CreateAsync(document);

                    return RedirectToAction("AssignedDocuments", "BusinessTypeTemplates", new { id = model.BusinessTypeTemplateID });
                }
                catch (Exception e)
                {
                    var errors = string.Join(",", e.Message);
                    ModelState.AddModelError(string.Empty, errors);
                }
            }

            ViewBag.DocumentationTemplateID = new SelectList(documentationTemplateRepository.UnAssignedDocumentationTemplates(model.BusinessTypeTemplateID), "DocumentationTemplateID", "DocumentationTemplateCode");
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // GET: BusinessTypeTemplates/DocumentsEdit
        [AuthorizeRoles(RoleNames.AdminRole)]
        public async Task<ActionResult> DocumentsEdit(int documentationBusinessTypeTemplateID)
        {
            DocumentationBusinessTypeTemplate docuBusinessType = await documentationBusinessTypeTemplateRepository.FindDocumentationBusinessTypeTemplatesByIDAsync(documentationBusinessTypeTemplateID);
            if (docuBusinessType == null)
            {
                return HttpNotFound();
            }

            ViewBag.BussinessTypeTemplateDescription = docuBusinessType.BusinessTypeTemplate.Description;

            //ViewBag.DocumentationTemplateID = new SelectList(documentationTemplateRepository.UnAssignedDocumentationTemplates(businessTypeTemplateID), "DocumentationTemplateID", "DocumentationTemplateCode",);
            //ViewBag.DocumentationImportanceID = new SelectList(documentationImportanceRepository.DocumentationImportances(), "DocumentationImportanceID", "Description", docuBusinessType.DocumentationImportanceID);
            ViewBag.DocumentationPeriodicityID = new SelectList(documentationPeriodicityRepository.DocumentationPeriodicitys(), "DocumentationPeriodicityID", "Description", docuBusinessType.DocumentationPeriodicityID);


            var model = new EditAssignDocumentTemplateViewModel(docuBusinessType);
            return View(model);
        }

        [HttpPost]
        [AuthorizeRoles(RoleNames.AdminRole)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DocumentsEdit(EditAssignDocumentTemplateViewModel model)
        {
            if (ModelState.IsValid)
            {
                DocumentationBusinessTypeTemplate docuBusinessType = model.GetDocument();
                try
                {
                    await documentationBusinessTypeTemplateRepository.UpdateAsync(docuBusinessType);

                    return RedirectToAction("AssignedDocuments", "BusinessTypeTemplates", new { id = model.BusinessTypeTemplateID });
                }
                catch (Exception e)
                {
                    var errors = string.Join(",", e.Message);
                    ModelState.AddModelError(string.Empty, errors);
                }
            }

            return View(model);
        }

        // GET: BusinessTypeTemplates/DocumentsDelete
        [AuthorizeRoles(RoleNames.AdminRole)]
        public async Task<ActionResult> DocumentsDelete(int documentationBusinessTypeTemplateID, int businessTypeTemplateID)
        {
            DocumentationBusinessTypeTemplate docuBusinessType = await documentationBusinessTypeTemplateRepository.FindDocumentationBusinessTypeTemplatesByIDAsync(documentationBusinessTypeTemplateID);
            if (docuBusinessType == null)
            {
                return HttpNotFound();
            }
            var model = new EditAssignDocumentTemplateViewModel(docuBusinessType);
            return View(model);
        }

        // POST: BusinessTypeTemplates/DocumentsDeleteConfirmed/5
        [HttpPost, ActionName("DocumentsDelete")]
        [AuthorizeRoles(RoleNames.AdminRole)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DocumentsDeleteConfirmed(int documentationBusinessTypeTemplateID, int businessTypeTemplateID)
        {
            await documentationBusinessTypeTemplateRepository.DeleteAsync(documentationBusinessTypeTemplateID);
            //return RedirectToAction("Index");
            return RedirectToAction("AssignedDocuments", "BusinessTypeTemplates", new { id = businessTypeTemplateID });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (businessTypeTemplateRepository != null)
                {
                    businessTypeTemplateRepository.Dispose();
                    businessTypeTemplateRepository = null;
                }

                if (documentationBusinessTypeTemplateRepository != null)
                {
                    documentationBusinessTypeTemplateRepository.Dispose();
                    documentationBusinessTypeTemplateRepository = null;
                }

                if (documentationTemplateRepository != null)
                {
                    documentationTemplateRepository.Dispose();
                    documentationTemplateRepository = null;
                }

                if (documentationPeriodicityRepository != null)
                {
                    documentationPeriodicityRepository.Dispose();
                    documentationPeriodicityRepository = null;
                }
                
            }
            base.Dispose(disposing);
        }
    }
}
