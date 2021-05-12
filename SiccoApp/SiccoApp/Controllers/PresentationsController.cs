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

namespace SiccoApp.Controllers
{
    public class PresentationsController : BaseController
    {
        //private SiccoAppContext db = new SiccoAppContext();
        private IPresentationRepository presentationRepository = null;
        private IDocumentFileService documentFileService = null;
        private IRequirementRepository requirementRepository = null;


        public PresentationsController(IPresentationRepository presentationRepo, IDocumentFileService documentFileServ, IRequirementRepository requirementRepo)
        {
            presentationRepository = presentationRepo;
            documentFileService = documentFileServ;
            requirementRepository = requirementRepo;
        }

        // GET: Presentations
        public ActionResult Index()
        {
            //var presentations = db.Presentations.Include(p => p.PresentationState).Include(p => p.Requirement);
            //return View(await presentations.ToListAsync());
            //var presentations = requirementRepository.FindPresentationsAsync()

            return View();
        }

        // GET: Presentations/Details/5
        public ActionResult Details(int? id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //Presentation presentation = await db.Presentations.FindAsync(id);
            //if (presentation == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(presentation);
            return View();
        }

        // GET: Presentations/Create
        public ActionResult Create()
        {
            //ViewBag.PresentationStateID = new SelectList(db.PresentationStates, "PresentationStateID", "Description");
            //ViewBag.RequirementID = new SelectList(db.Requirements, "RequirementID", "RequirementID");
            return View();
        }

        //public async Task<ActionResult> Create([Bind(Include = "PresentationID,RequirementID,PresentationStateID,PresentationDate,DocumentFiles,ApprovedFor,ApprovedDate,RejectedFor,RejectedDate,Observations")] Presentation presentation)
        // POST: Presentations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "PresentationID,PresentationStateID,Observations")] Presentation presentation, HttpPostedFileBase documentFiles)
        {
            if (ModelState.IsValid)
            {
                var requirement = await requirementRepository.FindRequirementByIDAsync(55);
                //presentation.Requirement = requirement;
                presentation.RequirementID = 55;
                presentation.DocumentFiles = await documentFileService.UploadDocumentFileAsync(documentFiles);
                await presentationRepository.CreateAsync(presentation);
                return RedirectToAction("Index");

                //db.Presentations.Add(presentation);
                //await db.SaveChangesAsync();
                //return RedirectToAction("Index");
            }

            //ViewBag.PresentationStateID = new SelectList(db.PresentationStates, "PresentationStateID", "Description", presentation.PresentationStateID);
            //ViewBag.RequirementID = new SelectList(db.Requirements, "RequirementID", "RequirementID", presentation.RequirementID);
            return View(presentation);
        }

        // GET: Presentations/Edit/5
        public ActionResult Edit(int? id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //Presentation presentation = await db.Presentations.FindAsync(id);
            //if (presentation == null)
            //{
            //    return HttpNotFound();
            //}
            //ViewBag.PresentationStateID = new SelectList(db.PresentationStates, "PresentationStateID", "Description", presentation.PresentationStateID);
            //ViewBag.RequirementID = new SelectList(db.Requirements, "RequirementID", "RequirementID", presentation.RequirementID);
            //return View(presentation);
            return View();
        }

        // POST: Presentations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PresentationID,RequirementID,PresentationStateID,PresentationDate,DocumentFiles,ApprovedFor,ApprovedDate,RejectedFor,RejectedDate,Observations")] Presentation presentation)
        {
            //if (ModelState.IsValid)
            //{
            //    db.Entry(presentation).State = EntityState.Modified;
            //    await db.SaveChangesAsync();
            //    return RedirectToAction("Index");
            //}
            //ViewBag.PresentationStateID = new SelectList(db.PresentationStates, "PresentationStateID", "Description", presentation.PresentationStateID);
            //ViewBag.RequirementID = new SelectList(db.Requirements, "RequirementID", "RequirementID", presentation.RequirementID);
            //return View(presentation);
            return View();
        }

        // GET: Presentations/Delete/5
        public ActionResult Delete(int? id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //Presentation presentation = await db.Presentations.FindAsync(id);
            //if (presentation == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(presentation);
            return View();
        }

        // POST: Presentations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //Presentation presentation = await db.Presentations.FindAsync(id);
            //db.Presentations.Remove(presentation);
            //await db.SaveChangesAsync();
            return RedirectToAction("Index");
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
                
            }
            base.Dispose(disposing);
        }
    }
}
