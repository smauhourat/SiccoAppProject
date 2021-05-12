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
    public class DocumentationTemplatesController : BaseController
    {
        private IDocumentationTemplateRepository documentationTemplateRepository = null;
        private IEntityTypeRepository entityTypeRepository = null;

        public DocumentationTemplatesController(IDocumentationTemplateRepository documentationTemplateRepo, IEntityTypeRepository entityTypeRepo)
        {
            documentationTemplateRepository = documentationTemplateRepo;
            entityTypeRepository = entityTypeRepo;
        }

        // GET: DocumentationTemplates
        [AuthorizeRoles(RoleNames.AdminRole)]
        public async Task<ActionResult> Index()
        {
            var documentationTemplates = await documentationTemplateRepository.FindDocumentationTemplatesAsync();
            return View(documentationTemplates.ToList());
        }

        // GET: DocumentationTemplates/Details/5
        [AuthorizeRoles(RoleNames.AdminRole)]
        public async Task<ActionResult> Details(int id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            DocumentationTemplate documentationTemplate = await documentationTemplateRepository.FindDocumentationTemplateByIDAsync(id);
            if (documentationTemplate == null)
            {
                return HttpNotFound();
            }
            return View(documentationTemplate);
        }

        // GET: DocumentationTemplates/Create
        [AuthorizeRoles(RoleNames.AdminRole)]
        public ActionResult Create()
        {
            ViewBag.EntityTypeID = new SelectList(entityTypeRepository.EntityTypes(), "EntityTypeID", "Description");
            return View();
        }

        // POST: DocumentationTemplates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [AuthorizeRoles(RoleNames.AdminRole)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "DocumentationTemplateID,DocumentationTemplateCode,Description,EntityTypeID,DocumentationPeriodicityID")] DocumentationTemplate documentationTemplate)
        {
            if (ModelState.IsValid)
            {
                await documentationTemplateRepository.CreateAsync(documentationTemplate);
                
                return RedirectToAction("Index");
            }

            ViewBag.EntityTypeID = new SelectList(entityTypeRepository.EntityTypes(), "EntityTypeID", "Description", documentationTemplate.EntityTypeID);
            return View(documentationTemplate);
        }

        // GET: DocumentationTemplates/Edit/5
        [AuthorizeRoles(RoleNames.AdminRole)]
        public async Task<ActionResult> Edit(int id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            DocumentationTemplate documentationTemplate = await documentationTemplateRepository.FindDocumentationTemplateByIDAsync(id);
            if (documentationTemplate == null)
            {
                return HttpNotFound();
            }

            ViewBag.EntityTypeID = new SelectList(entityTypeRepository.EntityTypes(), "EntityTypeID", "Description", documentationTemplate.EntityTypeID);

            return View(documentationTemplate);
        }

        // POST: DocumentationTemplates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [AuthorizeRoles(RoleNames.AdminRole)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "DocumentationTemplateID,DocumentationTemplateCode,Description,EntityTypeID,DocumentationPeriodicityID")] DocumentationTemplate documentationTemplate)
        {
            if (ModelState.IsValid)
            {
                await documentationTemplateRepository.UpdateAsync(documentationTemplate);
                return RedirectToAction("Index");
            }

            ViewBag.EntityTypeID = new SelectList(entityTypeRepository.EntityTypes(), "EntityTypeID", "Description", documentationTemplate.EntityTypeID);

            return View(documentationTemplate);
        }

        // GET: DocumentationTemplates/Delete/5
        [AuthorizeRoles(RoleNames.AdminRole)]
        public async Task<ActionResult> Delete(int id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            DocumentationTemplate documentationTemplate = await documentationTemplateRepository.FindDocumentationTemplateByIDAsync(id);
            if (documentationTemplate == null)
            {
                return HttpNotFound();
            }
            return View(documentationTemplate);
        }

        // POST: DocumentationTemplates/Delete/5
        [HttpPost, ActionName("Delete")]
        [AuthorizeRoles(RoleNames.AdminRole)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await documentationTemplateRepository.DeleteAsync(id);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (documentationTemplateRepository != null)
                {
                    documentationTemplateRepository.Dispose();
                    documentationTemplateRepository = null;
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
