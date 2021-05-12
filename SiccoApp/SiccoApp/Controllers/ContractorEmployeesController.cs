using SiccoApp.Helpers;
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
    public class ContractorEmployeesController : BaseController
    {
        private IContractorRepository contractorRepository = null;
        private IEmployeeRepository employeeRepository = null;
        private IEmployeeRelationshipTypeRepository employeeRelationshipTypeRepository = null;
        private IWorkflowMessageService workflowMessageService = null;

        public ContractorEmployeesController(IContractorRepository contractorRepo, IEmployeeRepository employeeRepo, IEmployeeRelationshipTypeRepository employeeRelationshipTypeRepo,  IWorkflowMessageService workflowMessageServ)
        {
            contractorRepository = contractorRepo;
            employeeRepository = employeeRepo;
            employeeRelationshipTypeRepository = employeeRelationshipTypeRepo;
            workflowMessageService = workflowMessageServ;
        }

        // GET: ContractorEmployees
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

            var model = new ContractorEmployeesViewModel(contractor);
            return View(model);
        }

        // GET: ContractorEmployees/Edit
        [AuthorizeRoles(RoleNames.ContractorRole)]
        public async Task<ActionResult> Edit(int employeeID)
        {
            Employee employee = await employeeRepository.FindEmployeeByIDAsync(employeeID);
            if (employee == null)
            {
                return HttpNotFound();
            }
            var model = new EditContractorEmployeesViewModel(employee);
            model.QRCode = QRCodeGenerator.GenerateQRCodeInMemory(employee.FullName + " - " + employee.IdentificationNumber.ToString());
            ViewBag.EmployeeRelationshipTypeID = new SelectList(employeeRelationshipTypeRepository.EmployeeRelationshipTypes(), "EmployeeRelationshipTypeID", "Description", employee.EmployeeRelationshipTypeID);
            return View(model);
        }

        [HttpPost]
        [AuthorizeRoles(RoleNames.ContractorRole)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditContractorEmployeesViewModel model)
        {
            if (ModelState.IsValid)
            {
                Employee employee = model.GetEmployee();
                try
                {
                    employee.ModifiedDate = DateTime.UtcNow;
                    employee.ModifiedUser = System.Web.HttpContext.Current.User.Identity.Name;
                    await employeeRepository.UpdateAsync(employee);

                    return RedirectToAction("Index", "ContractorEmployees", new { id = model.ContractorID });
                }
                catch (Exception e)
                {
                    var errors = string.Join(",", e.Message);
                    ModelState.AddModelError(string.Empty, errors);
                }

                ViewBag.EmployeeRelationshipTypeID = new SelectList(employeeRelationshipTypeRepository.EmployeeRelationshipTypes(), "EmployeeRelationshipTypeID", "Description", employee.EmployeeRelationshipTypeID);
            }

            return View(model);
        }

        // GET: ContractorEmployees/Create
        [AuthorizeRoles(RoleNames.ContractorRole)]
        public ActionResult Create(int contractorID)
        {
            EditContractorEmployeesViewModel model = new EditContractorEmployeesViewModel();
            model.ContractorID = contractorID;
            ViewBag.EmployeeRelationshipTypeID = new SelectList(employeeRelationshipTypeRepository.EmployeeRelationshipTypes(), "EmployeeRelationshipTypeID", "Description");
            return View(model);
        }

        [HttpPost]
        [AuthorizeRoles(RoleNames.ContractorRole)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(EditContractorEmployeesViewModel model)
        {

            if (ModelState.IsValid)
            {
                Employee employee = (Employee)model.GetEmployee();
                employee.ContractorID = model.ContractorID;

                try
                {
                    employee.CreationDate = DateTime.UtcNow;
                    employee.CreationUser = System.Web.HttpContext.Current.User.Identity.Name;
                    await employeeRepository.CreateAsync(employee);

                    return RedirectToAction("Index", "ContractorEmployees", new { id = model.ContractorID });
                }
                catch (Exception e)
                {
                    var errors = string.Join(",", e.Message);
                    ModelState.AddModelError(string.Empty, errors);
                }

                ViewBag.EmployeeRelationshipTypeID = new SelectList(employeeRelationshipTypeRepository.EmployeeRelationshipTypes(), "EmployeeRelationshipTypeID", "Description", employee.EmployeeRelationshipTypeID);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // GET: ContractorEmployees/Delete
        [AuthorizeRoles(RoleNames.ContractorRole)]
        public async Task<ActionResult> Delete(int employeeID, int contractorID)
        {
            Employee employee = await employeeRepository.FindEmployeeByIDAsync(employeeID);
            if (employee == null)
            {
                return HttpNotFound();
            }
            var model = new EditContractorEmployeesViewModel(employee);
            return View(model);
        }

        // POST: ContractorEmployees/DeleteConfirmed/5
        [HttpPost, ActionName("Delete")]
        [AuthorizeRoles(RoleNames.ContractorRole)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int employeeID, int contractorID)
        {
            await employeeRepository.DeleteAsync(employeeID);
            //return RedirectToAction("Index");
            return RedirectToAction("Index", "ContractorEmployees", new { id = contractorID });
        }

        [AuthorizeRoles(RoleNames.ContractorRole)]
        public async Task<ActionResult> Credential(int employeeID)
        {
            Employee employee = await employeeRepository.FindEmployeeByIDAsync(employeeID);
            if (employee == null)
            {
                return HttpNotFound();
            }
            var model = new EditContractorEmployeesViewModel(employee);
            model.QRCode = QRCodeGenerator.GenerateQRCodeInMemory(model.QRCodeText);
            ViewBag.EmployeeRelationshipTypeID = new SelectList(employeeRelationshipTypeRepository.EmployeeRelationshipTypes(), "EmployeeRelationshipTypeID", "Description", employee.EmployeeRelationshipTypeID);

            return PartialView("_Credential", model);
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

                if (employeeRepository != null)
                {
                    employeeRepository.Dispose();
                    employeeRepository = null;
                }

                if (employeeRelationshipTypeRepository != null)
                {
                    employeeRelationshipTypeRepository.Dispose();
                    employeeRelationshipTypeRepository = null;
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