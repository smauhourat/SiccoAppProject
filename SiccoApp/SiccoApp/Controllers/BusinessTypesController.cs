using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
//using SiccoApp.DAL;
using SiccoApp.Persistence;
using System.Threading.Tasks;
using Resources;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SiccoApp.Models;

namespace SiccoApp.Controllers
{
    /// <summary>
    /// Maneja los Rubros pero de un determinado Cliente
    /// </summary>
    public class BusinessTypesController : BaseController
    {
        private IBusinessTypeRepository businessTypeRepository = null;
        private IDocumentationBusinessTypeRepository documentationBusinessTypeRepository = null;
        private IDocumentationRepository documentationRepository = null;
        private IDocumentationPeriodicityRepository documentationPeriodicityRepository = null;

        public BusinessTypesController(
            IBusinessTypeRepository businessTypeRepo, 
            IDocumentationBusinessTypeRepository documentationBusinessTypeRepo, 
            IDocumentationRepository documentationRepo,
            IDocumentationPeriodicityRepository documentationPeriodicityRepo)
        {
            businessTypeRepository = businessTypeRepo;
            documentationBusinessTypeRepository = documentationBusinessTypeRepo;
            documentationRepository = documentationRepo;
            documentationPeriodicityRepository = documentationPeriodicityRepo;
        }

        // GET: BusinessType
        [AuthorizeRoles(RoleNames.CustomerRole)]
        public async Task<ActionResult> Index()
        {
            var businessTypes = await businessTypeRepository.FindBusinessTypesAsync(base.CurrentCustomerID);

            return View(businessTypes.ToList());
        }

        // GET: BusinessType/Details/5
        [AuthorizeRoles(RoleNames.CustomerRole)]
        public async Task<ActionResult> Details(int id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            BusinessType businessType = await businessTypeRepository.FindBusinessTypeByIDAsync(id);
            if (businessType == null)
            {
                return HttpNotFound();
            }
            return View(businessType);
        }

        // GET: BusinessTypeTemplates/Create
        [AuthorizeRoles(RoleNames.CustomerRole)]
        public ActionResult Create()
        {
            return View();
        }

        // POST: BusinessType/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [AuthorizeRoles(RoleNames.CustomerRole)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "BusinessTypeID,BusinessTypeCode,Description")] BusinessType businessType)
        {
            businessType.CustomerID = base.CurrentCustomerID;
            if (ModelState.IsValid)
            {
                await businessTypeRepository.CreateAsync(businessType);
                return RedirectToAction("Index");
            }

            return View(businessType);
        }

        // GET: BusinessType/Edit/5
        [AuthorizeRoles(RoleNames.CustomerRole)]
        public async Task<ActionResult> Edit(int id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            BusinessType businessType = await businessTypeRepository.FindBusinessTypeByIDAsync(id);
            if (businessType == null)
            {
                return HttpNotFound();
            }
            return View(businessType);
        }

        // POST: BusinessTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [AuthorizeRoles(RoleNames.CustomerRole)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "BusinessTypeID,CustomerID,BusinessTypeCode,Description")] BusinessType businessType)
        {
            if (ModelState.IsValid)
            {
                await businessTypeRepository.UpdateAsync(businessType);

                return RedirectToAction("Index");
            }
            return View(businessType);
        }

        // GET: BusinessTypes/Delete/5
        [AuthorizeRoles(RoleNames.CustomerRole)]
        public async Task<ActionResult> Delete(int id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            BusinessType businessType = await businessTypeRepository.FindBusinessTypeByIDAsync(id);
            if (businessType == null)
            {
                return HttpNotFound();
            }
            return View(businessType);
        }

        // POST: BusinessTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [AuthorizeRoles(RoleNames.CustomerRole)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await businessTypeRepository.DeleteAsync(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        // GET: BusinessTypes/AssignedDocuments/5
        [AuthorizeRoles(RoleNames.CustomerRole)]
        public async Task<ActionResult> AssignedDocuments(int id)
        {
            var documentationBusinessTypes = await documentationBusinessTypeRepository.FindDocumentationBusinessTypesByBTAsync(id);

            BusinessType businessType = await businessTypeRepository.FindBusinessTypeByIDAsync(id);
            if (businessType == null)
            {
                return HttpNotFound();
            }

            var viewModel = new AssignedDocumentsViewModel(businessType, documentationBusinessTypes);

            return View(viewModel);

        }

        // GET: BusinessTypes/DocumentsCreate
        [AuthorizeRoles(RoleNames.CustomerRole)]
        public ActionResult DocumentsCreate(int businessTypeID)
        {
            EditAssignDocumentViewModel model = new EditAssignDocumentViewModel();
            model.BusinessTypeID = businessTypeID;

            ViewBag.DocumentationID = new SelectList(documentationRepository.UnAssignedDocumentations(businessTypeID, base.CurrentCustomerID), "DocumentationID", "DocumentationCode");
            //ViewBag.DocumentationImportanceID = new SelectList(documentationImportanceRepository.DocumentationImportances(), "DocumentationImportanceID", "Description");
            ViewBag.DocumentationPeriodicityID = new SelectList(documentationPeriodicityRepository.DocumentationPeriodicitys(), "DocumentationPeriodicityID", "Description");

            return View(model);
        }

        [HttpPost]
        [AuthorizeRoles(RoleNames.CustomerRole)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DocumentsCreate(EditAssignDocumentViewModel model)
        {

            if (ModelState.IsValid)
            {
                DocumentationBusinessType document = (DocumentationBusinessType)model.GetDocument();
                //document.BusinessTypeTemplateID = model.BusinessTypeTemplateID;

                try
                {
                    await documentationBusinessTypeRepository.CreateAsync(document);

                    return RedirectToAction("AssignedDocuments", "BusinessTypes", new { id = model.BusinessTypeID });
                }
                catch (Exception e)
                {
                    var errors = string.Join(",", e.Message);
                    ModelState.AddModelError(string.Empty, errors);
                }
            }

            ViewBag.DocumentationID = new SelectList(documentationRepository.UnAssignedDocumentations(model.BusinessTypeID, base.CurrentCustomerID), "DocumentationID", "DocumentationCode");
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // GET: BusinessTypes/DocumentsEdit
        [AuthorizeRoles(RoleNames.CustomerRole)]
        public async Task<ActionResult> DocumentsEdit(int documentationBusinessTypeID)
        {
            DocumentationBusinessType docuBusinessType = await documentationBusinessTypeRepository.FindDocumentationBusinessTypesByIDAsync(documentationBusinessTypeID);
            if (docuBusinessType == null)
            {
                return HttpNotFound();
            }

            ViewBag.DocumentationPeriodicityID = new SelectList(documentationPeriodicityRepository.DocumentationPeriodicitys(), "DocumentationPeriodicityID", "Description", docuBusinessType.DocumentationPeriodicityID);

            var model = new EditAssignDocumentViewModel(docuBusinessType);
            return View(model);
        }

        [HttpPost]
        [AuthorizeRoles(RoleNames.CustomerRole)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DocumentsEdit(EditAssignDocumentViewModel model)
        {
            if (ModelState.IsValid)
            {
                DocumentationBusinessType docuBusinessType = model.GetDocument();
                try
                {
                    await documentationBusinessTypeRepository.UpdateAsync(docuBusinessType);

                    return RedirectToAction("AssignedDocuments", "BusinessTypes", new { id = model.BusinessTypeID });
                }
                catch (Exception e)
                {
                    var errors = string.Join(",", e.Message);
                    ModelState.AddModelError(string.Empty, errors);
                }
            }

            return View(model);
        }

        // GET: BusinessTypes/DocumentsDelete
        [AuthorizeRoles(RoleNames.CustomerRole)]
        public async Task<ActionResult> DocumentsDelete(int documentationBusinessTypeID, int businessTypeID)
        {
            DocumentationBusinessType docuBusinessType = await documentationBusinessTypeRepository.FindDocumentationBusinessTypesByIDAsync(documentationBusinessTypeID);
            if (docuBusinessType == null)
            {
                return HttpNotFound();
            }
            var model = new EditAssignDocumentViewModel(docuBusinessType);
            return View(model);
        }

        // POST: BusinessTypes/DocumentsDeleteConfirmed/5
        [HttpPost, ActionName("DocumentsDelete")]
        [AuthorizeRoles(RoleNames.CustomerRole)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DocumentsDeleteConfirmed(int documentationBusinessTypeID, int businessTypeID)
        {
            await documentationBusinessTypeRepository.DeleteAsync(documentationBusinessTypeID);
            //return RedirectToAction("Index");
            return RedirectToAction("AssignedDocuments", "BusinessTypes", new { id = businessTypeID });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (businessTypeRepository != null)
                {
                    businessTypeRepository.Dispose();
                }
                
                if (documentationBusinessTypeRepository != null)
                {
                    documentationBusinessTypeRepository.Dispose();
                    documentationBusinessTypeRepository = null;
                }
                
                if (documentationRepository != null)
                {
                    documentationRepository.Dispose();
                    documentationRepository = null;
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
