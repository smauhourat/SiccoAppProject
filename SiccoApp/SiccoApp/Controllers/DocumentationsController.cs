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
    public class DocumentationsController : BaseController
    {
        private IDocumentationRepository documentationRepository = null;
        private IEntityTypeRepository entityTypeRepository = null;

        public DocumentationsController(IDocumentationRepository documentationRepo, IEntityTypeRepository entityTypeRepo)
        {
            documentationRepository = documentationRepo;
            entityTypeRepository = entityTypeRepo;
        }

        // GET: Documentations
        [AuthorizeRoles(RoleNames.CustomerRole)]
        public async Task<ActionResult> Index()
        {
            var documentations = await documentationRepository.FindDocumentationsAsync(base.CurrentCustomerID);
            return View(documentations.ToList());
        }

        // GET: Documentations/Details/5
        [AuthorizeRoles(RoleNames.CustomerRole)]
        public async Task<ActionResult> Details(int id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            Documentation documentation = await documentationRepository.FindDocumentationByIDAsync(id);
            if (documentation == null)
            {
                return HttpNotFound();
            }
            return View(documentation);
        }

        // GET: Documentations/Create
        [AuthorizeRoles(RoleNames.CustomerRole)]
        public ActionResult Create()
        {
            ViewBag.EntityTypeID = new SelectList(entityTypeRepository.EntityTypes(), "EntityTypeID", "Description");
            return View();
        }

        // POST: Documentatios/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [AuthorizeRoles(RoleNames.CustomerRole)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "DocumentationID,DocumentationCode,Description,EntityTypeID,DocumentationPeriodicityID")] Documentation documentation)
        {
            documentation.CustomerID = base.CurrentCustomerID;
            if (ModelState.IsValid)
            {
                await documentationRepository.CreateAsync(documentation);
                
                return RedirectToAction("Index");
            }

            ViewBag.EntityTypeID = new SelectList(entityTypeRepository.EntityTypes(), "EntityTypeID", "Description", documentation.EntityTypeID);
            return View(documentation);
        }

        // GET: Documentations/Edit/5
        [AuthorizeRoles(RoleNames.CustomerRole)]
        public async Task<ActionResult> Edit(int id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            Documentation documentation = await documentationRepository.FindDocumentationByIDAsync(id);
            if (documentation == null)
            {
                return HttpNotFound();
            }

            ViewBag.EntityTypeID = new SelectList(entityTypeRepository.EntityTypes(), "EntityTypeID", "Description", documentation.EntityTypeID);

            return View(documentation);
        }

        // POST: Documentations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [AuthorizeRoles(RoleNames.CustomerRole)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "DocumentationID,CustomerID,DocumentationCode,Description,EntityTypeID,DocumentationPeriodicityID")] Documentation documentation)
        {
            if (ModelState.IsValid)
            {
                await documentationRepository.UpdateAsync(documentation);
                return RedirectToAction("Index");
            }

            ViewBag.EntityTypeID = new SelectList(entityTypeRepository.EntityTypes(), "EntityTypeID", "Description", documentation.EntityTypeID);

            return View(documentation);
        }

        // GET: Documentations/Delete/5
        [AuthorizeRoles(RoleNames.CustomerRole)]
        public async Task<ActionResult> Delete(int id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            Documentation documentation = await documentationRepository.FindDocumentationByIDAsync(id);
            if (documentation == null)
            {
                return HttpNotFound();
            }
            return View(documentation);
        }

        // POST: Documentations/Delete/5
        [HttpPost, ActionName("Delete")]
        [AuthorizeRoles(RoleNames.CustomerRole)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await documentationRepository.DeleteAsync(id);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (documentationRepository != null)
                {
                    documentationRepository.Dispose();
                    documentationRepository = null;
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
