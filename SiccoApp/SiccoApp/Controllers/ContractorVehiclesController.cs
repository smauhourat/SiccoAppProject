using SiccoApp.Models;
using SiccoApp.Persistence;
using SiccoApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SiccoApp.Controllers
{
    public class ContractorVehiclesController : BaseController
    {
        private IContractorRepository contractorRepository = null;
        private IVehicleRepository vehicleRepository = null;
        private IWorkflowMessageService workflowMessageService = null;

        public ContractorVehiclesController(IContractorRepository contractorRepo, IVehicleRepository vehicleRepo, IWorkflowMessageService workflowMessageServ)
        {
            contractorRepository = contractorRepo;
            vehicleRepository = vehicleRepo;
            workflowMessageService = workflowMessageServ;
        }

        // GET: ContractorVehicles
        [AuthorizeRoles(RoleNames.ContractorRole)]
        public async Task<ActionResult> Index()
        {
            Contractor contractor = await contractorRepository.FindContractorsByIDAsync(base.CurrentContractorID);
            if (contractor == null)
            {
                return HttpNotFound();
            }

            if (contractor.CustomerID != base.CurrentCustomerID)
            {
                return HttpNotFound();
            }

            var model = new ContractorVehiclesViewModel(contractor);
            return View(model);
        }

        // GET: ContractorVehicles/Edit
        [AuthorizeRoles(RoleNames.ContractorRole)]
        public async Task<ActionResult> Edit(int vehicleID)
        {
            Vehicle vehicle = await vehicleRepository.FindVehicleByIDAsync(vehicleID);
            if (vehicle == null)
            {
                return HttpNotFound();
            }
            var model = new EditContractorVehiclesViewModel(vehicle);
            return View(model);
        }

        [HttpPost]
        [AuthorizeRoles(RoleNames.ContractorRole)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditContractorVehiclesViewModel model)
        {
            if (ModelState.IsValid)
            {
                Vehicle vehicle = model.GetVehicle();
                try
                {
                    vehicle.ModifiedDate = DateTime.UtcNow;
                    vehicle.ModifiedUser = System.Web.HttpContext.Current.User.Identity.Name;
                    await vehicleRepository.UpdateAsync(vehicle);

                    return RedirectToAction("Index", "ContractorVehicles", new { id = model.ContractorID });
                }
                catch (Exception e)
                {
                    var errors = string.Join(",", e.Message);
                    ModelState.AddModelError(string.Empty, errors);
                }
            }

            return View(model);
        }

        // GET: ContractorVehicles/Create
        [AuthorizeRoles(RoleNames.ContractorRole)]
        public ActionResult Create(int contractorID)
        {
            EditContractorVehiclesViewModel model = new EditContractorVehiclesViewModel();
            model.ContractorID = contractorID;
            return View(model);
        }

        [HttpPost]
        [AuthorizeRoles(RoleNames.ContractorRole)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(EditContractorVehiclesViewModel model)
        {

            if (ModelState.IsValid)
            {
                Vehicle vehicle = (Vehicle)model.GetVehicle();
                vehicle.ContractorID = model.ContractorID;

                try
                {
                    vehicle.CreationDate = DateTime.UtcNow;
                    vehicle.CreationUser = System.Web.HttpContext.Current.User.Identity.Name;
                    await vehicleRepository.CreateAsync(vehicle);

                    return RedirectToAction("Index", "ContractorVehicles", new { id = model.ContractorID });
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

        // GET: ContractorVehicles/Delete
        [AuthorizeRoles(RoleNames.ContractorRole)]
        public async Task<ActionResult> Delete(int vehicleID, int contractorID)
        {
            Vehicle vehicle = await vehicleRepository.FindVehicleByIDAsync(vehicleID);
            if (vehicle == null)
            {
                return HttpNotFound();
            }
            var model = new EditContractorVehiclesViewModel(vehicle);
            return View(model);
        }

        // POST: ContractorVehicles/DeleteConfirmed/5
        [HttpPost, ActionName("Delete")]
        [AuthorizeRoles(RoleNames.ContractorRole)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int vehicleID, int contractorID)
        {
            await vehicleRepository.DeleteAsync(vehicleID);
            //return RedirectToAction("Index");
            return RedirectToAction("Index", "ContractorVehicles", new { id = contractorID });
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
                
                if (vehicleRepository != null)
                {
                    vehicleRepository.Dispose();
                    vehicleRepository = null;
                }
                
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