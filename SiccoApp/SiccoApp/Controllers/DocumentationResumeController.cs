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
using SiccoApp.Services;

namespace SiccoApp.Controllers
{
    public class DocumentationResumeController : BaseController
    {
        private IDocumentationRepository documentationRepository = null;

        public DocumentationResumeController(IDocumentationRepository documentationRepo)
        {
            documentationRepository = documentationRepo;
        }

        [AuthorizeRoles(RoleNames.ContractorRole)]
        public async Task<ActionResult> Index()
        {
            var documentationResume = await documentationRepository.FindDocumentationResumeByCustomerContractorAsync(base.CurrentCustomerID, base.CurrentContractorID);
            return View(new DocumentationResumeViewModel(documentationResume.ToList()));
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
            }
            base.Dispose(disposing);
        }
    }
}