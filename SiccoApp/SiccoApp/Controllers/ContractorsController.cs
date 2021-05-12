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
using Microsoft.Owin.Security.DataProtection;
using Microsoft.AspNet.Identity.Owin;

namespace SiccoApp.Controllers
{
    public class ContractorsController : BaseController
    {
        private IContractorRepository contractorRepository = null;
        private ILocalizationRepository localizationRepository = null;
        private IBusinessTypeRepository businessTypeRepository = null;
        private IEmployeeRepository employeeRepository = null;
        private IVehicleRepository vehicleRepository = null;
        private IContractRepository contractRepository = null;
        private IEmployeeRelationshipTypeRepository employeeRelationshipTypeRepository = null;
        private IIdentityMessageService emailService = null;
        private IWorkflowMessageService workflowMessageService = null;

        //ojoooo no me gusta
        private UserManager<ContractorUser> userManager = null;

        public ContractorsController(IContractorRepository contractorRepo, 
            ILocalizationRepository localizationRepo, 
            IBusinessTypeRepository businessTypeRepo, 
            IEmployeeRepository employeeTypesRepo, 
            IVehicleRepository vehicleRepo, 
            IContractRepository contractRepo, 
            IEmployeeRelationshipTypeRepository employeeRelationshipTypeRepo, 
            IWorkflowMessageService workflowMessageServ, 
            IIdentityMessageService emailServ)
        {
            contractorRepository = contractorRepo;
            localizationRepository = localizationRepo;
            businessTypeRepository = businessTypeRepo;
            employeeRepository = employeeTypesRepo;
            vehicleRepository = vehicleRepo;
            contractRepository = contractRepo;
            employeeRelationshipTypeRepository = employeeRelationshipTypeRepo;

            emailService = emailServ;

            workflowMessageService = workflowMessageServ;

            userManager = new UserManager<ContractorUser>(new UserStore<ContractorUser>(new ApplicationDbContext()));
            userManager.UserValidator = new UserValidator<ContractorUser>(userManager)
            {
                AllowOnlyAlphanumericUserNames = true,
                RequireUniqueEmail = true
            };

            var provider = new DpapiDataProtectionProvider("SampleAppName");
            userManager.UserTokenProvider = new DataProtectorTokenProvider<ContractorUser>(provider.Create("SampleTokenName"));
            userManager.EmailService = emailService;
        }

        // GET: Contractors
        [AuthorizeRoles(RoleNames.CustomerRole)]
        public async Task<ActionResult> Index()
        {
            var contractors = await contractorRepository.FindContractorsAsync(base.CurrentCustomerID);
            return View(contractors.ToList());
        }

        // GET: Contractors/Details/5
        [AuthorizeRoles(RoleNames.CustomerRole)]
        public async Task<ActionResult> Details(int id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            Contractor contractor = await contractorRepository.FindContractorsByIDAsync(id);
            if (contractor == null)
            {
                return HttpNotFound();
            }
            return View(contractor);
        }

        // GET: Contractors/Create
        [AuthorizeRoles(RoleNames.CustomerRole)]
        public ActionResult Create()
        {
            ViewBag.BusinessTypeID = new SelectList(businessTypeRepository.BusinessTypes(base.CurrentCustomerID), "BusinessTypeID", "BusinessTypeCode");
            ViewBag.CountryID = new SelectList(localizationRepository.Countries(), "CountryID", "CountryName");
            return View();
        }

        // POST: Contractors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [AuthorizeRoles(RoleNames.CustomerRole)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ContractorID,CustomerID,BusinessTypeID,CompanyName,TaxIdNumber,CountryID,StateID,City,Address,PhoneNumber,EmergencyPhoneNumber,Email")] Contractor contractor)
        {
            if (ModelState.IsValid)
            {
                contractor.CustomerID = base.CurrentCustomerID;
                contractor.CreationDate = DateTime.UtcNow;
                contractor.CreationUser = System.Web.HttpContext.Current.User.Identity.Name;
                await contractorRepository.CreateAsync(contractor);
                return RedirectToAction("Index");
                //
            }

            ViewBag.BusinessTypeID = new SelectList(businessTypeRepository.BusinessTypes(base.CurrentCustomerID), "BusinessTypeID", "BusinessTypeCode", contractor.BusinessTypeID);
            ViewBag.CountryID = new SelectList(localizationRepository.Countries(), "CountryID", "CountryName", contractor.CountryID);
            return View(contractor);
        }

        // GET: Contractors/Edit/5
        [AuthorizeRoles(RoleNames.CustomerRole)]
        public async Task<ActionResult> Edit(int id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            Contractor contractor = await contractorRepository.FindContractorsByIDAsync(id);
            if (contractor == null)
            {
                return HttpNotFound();
            }
            ViewBag.BusinessTypeID = new SelectList(businessTypeRepository.BusinessTypes(base.CurrentCustomerID), "BusinessTypeID", "BusinessTypeCode", contractor.BusinessTypeID);
            ViewBag.CountryID = new SelectList(localizationRepository.Countries(), "CountryID", "CountryName", contractor.CountryID);
            ViewBag.StateID = new SelectList(localizationRepository.States(contractor.CountryID), "StateID", "StateName", contractor.StateID);
            return View(contractor);
        }

        // POST: Contractors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [AuthorizeRoles(RoleNames.CustomerRole)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ContractorID,CustomerID,BusinessTypeID,CompanyName,TaxIdNumber,CountryID,StateID,City,Address,PhoneNumber,EmergencyPhoneNumber,Email,CreationDate,CreationUser,ModifiedDate,ModifiedUser")] Contractor contractor)
        {
            if (ModelState.IsValid)
            {
                contractor.ModifiedDate = DateTime.UtcNow;
                contractor.ModifiedUser = System.Web.HttpContext.Current.User.Identity.Name;
                await contractorRepository.UpdateAsync(contractor);

                return RedirectToAction("Index");
            }
            ViewBag.BusinessTypeID = new SelectList(businessTypeRepository.BusinessTypes(base.CurrentCustomerID), "BusinessTypeID", "BusinessTypeCode", contractor.BusinessTypeID);
            ViewBag.CountryID = new SelectList(localizationRepository.Countries(), "CountryID", "CountryName", contractor.CountryID);
            ViewBag.StateID = new SelectList(localizationRepository.States(), "StateID", "StateName", contractor.StateID);
            return View(contractor);
        }

        // GET: Contractors/Delete/5
        [AuthorizeRoles(RoleNames.CustomerRole)]
        public async Task<ActionResult> Delete(int id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            Contractor contractor = await contractorRepository.FindContractorsByIDAsync(id);
            if (contractor == null)
            {
                return HttpNotFound();
            }
            return View(contractor);
        }

        // POST: Contractors/Delete/5
        [HttpPost, ActionName("Delete")]
        [AuthorizeRoles(RoleNames.CustomerRole)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await contractorRepository.DeleteAsync(id);
            return RedirectToAction("Index");
        }

        #region " Users Region "

        // GET: Customers/Users/5
        [AuthorizeRoles(RoleNames.CustomerRole)]
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Users(int id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            Contractor contractor = await contractorRepository.FindContractorsByIDAsync(id, base.CurrentCustomerID);
            if (contractor == null)
            {
                return HttpNotFound();
            }

            //if (contractor.CustomerID != base.CurrentCustomerID)
            //{
            //    return HttpNotFound();
            //}

            var model = new ContractorUsersViewModel(contractor);
            return View(model);
        }

        // GET: Customers/UsersCreate
        [AuthorizeRoles(RoleNames.CustomerRole)]
        public ActionResult UsersCreate(int contractorID)
        {
            RegisterContractorUserViewModel model = new RegisterContractorUserViewModel();
            model.ContractorID = contractorID;
            return View(model);
        }

        [HttpPost]
        [AuthorizeRoles(RoleNames.CustomerRole)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UsersCreate(RegisterContractorUserViewModel model)
        {

            if (ModelState.IsValid)
            {
                ContractorUser user = (ContractorUser)model.GetUser();
                user.ContractorID = model.ContractorID;
                
                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    var resultRole = userManager.AddToRole(user.Id, "ContractorRole");

                    if (resultRole.Succeeded)
                    {
                        await workflowMessageService.SendContractorUserRegistrationNotificationMessageAsync(model);

                        // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                        // Send an email with this link
                        //string code = await userManager.GenerateEmailConfirmationTokenAsync(user.Id);
                        //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                        //await userManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
                        await workflowMessageService.SendUserRegistrationMessageAsync(userManager, user.Id, Request.Url.Scheme, Url);

                        return RedirectToAction("Users", "Contractors", new { id = model.ContractorID });
                    }
                    else
                    {
                        var errors = string.Join(",", resultRole.Errors);
                        ModelState.AddModelError(string.Empty, errors);
                    }
                }
                else
                {
                    var errors = string.Join(",", result.Errors);
                    ModelState.AddModelError(string.Empty, errors);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // GET: Customers/Delete/5
        [AuthorizeRoles(RoleNames.CustomerRole)]
        public ActionResult UsersDelete(string username)
        {
            if ((username == null) || (username.ToString().Length == 0))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ContractorUser contractoruser = userManager.FindByName(username);
            if (contractoruser == null)
            {
                return HttpNotFound();
            }

            var model = new EditContractorUserViewModel((ContractorUser)contractoruser);
            return View(model);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("UsersDelete")]
        [AuthorizeRoles(RoleNames.CustomerRole)]
        [ValidateAntiForgeryToken]
        public ActionResult UsersDeleteConfirmed(string username)
        {
            ContractorUser contractoruser = userManager.FindByName(username);
            if (contractoruser == null)
            {
                return HttpNotFound();
            }

            var result = userManager.Delete(contractoruser);
            if (result.Succeeded)
            {
                return RedirectToAction("Users", "Contractors", new { id = contractoruser.ContractorID });
            }

            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);

        }

        #endregion

        #region " Employee Region "

        // GET: Contractors/Employees/5
        [AuthorizeRoles(RoleNames.CustomerRole)]
        public async Task<ActionResult> Employees(int id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            Contractor contractor = await contractorRepository.FindContractorsByIDAsync(id);
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

        // GET: Contractors/EmployeeEdit
        [AuthorizeRoles(RoleNames.CustomerRole)]
        public async Task<ActionResult> EmployeesEdit(int employeeID)
        {
            Employee employee = await employeeRepository.FindEmployeeByIDAsync(employeeID);
            if (employee == null)
            {
                return HttpNotFound();
            }
            var model = new EditContractorEmployeesViewModel(employee);
            ViewBag.EmployeeRelationshipTypeID = new SelectList(employeeRelationshipTypeRepository.EmployeeRelationshipTypes(), "EmployeeRelationshipTypeID", "Description", employee.EmployeeRelationshipTypeID);
            return View(model);
        }

        [HttpPost]
        [AuthorizeRoles(RoleNames.CustomerRole)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EmployeesEdit(EditContractorEmployeesViewModel model)
        {
            if (ModelState.IsValid)
            {
                Employee employee = model.GetEmployee();
                try
                {
                    employee.ModifiedDate = DateTime.UtcNow;
                    employee.ModifiedUser = System.Web.HttpContext.Current.User.Identity.Name;
                    await employeeRepository.UpdateAsync(employee);    

                    return RedirectToAction("Employees", "Contractors", new { id = model.ContractorID });
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

        // GET: Contractors/EmployeeCreate
        [AuthorizeRoles(RoleNames.CustomerRole)]
        public ActionResult EmployeesCreate(int contractorID)
        {
            EditContractorEmployeesViewModel model = new EditContractorEmployeesViewModel();
            model.ContractorID = contractorID;
            ViewBag.EmployeeRelationshipTypeID = new SelectList(employeeRelationshipTypeRepository.EmployeeRelationshipTypes(), "EmployeeRelationshipTypeID", "Description");
            return View(model);
        }

        [HttpPost]
        [AuthorizeRoles(RoleNames.CustomerRole)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EmployeesCreate(EditContractorEmployeesViewModel model)
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
     
                    return RedirectToAction("Employees", "Contractors", new { id = model.ContractorID });
                }
                catch(Exception e)
                {
                    var errors = string.Join(",", e.Message);
                    ModelState.AddModelError(string.Empty, errors);
                }

                ViewBag.EmployeeRelationshipTypeID = new SelectList(employeeRelationshipTypeRepository.EmployeeRelationshipTypes(), "EmployeeRelationshipTypeID", "Description", employee.EmployeeRelationshipTypeID);
            }

            
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // GET: Contractors/EmployeesDelete
        [AuthorizeRoles(RoleNames.CustomerRole)]
        public async Task<ActionResult> EmployeesDelete(int employeeID, int contractorID)
        {
            Employee employee = await employeeRepository.FindEmployeeByIDAsync(employeeID);
            if (employee == null)
            {
                return HttpNotFound();
            }
            var model = new EditContractorEmployeesViewModel(employee);
            return View(model);
        }

        // POST: Contractors/EmployeesDeleteConfirmed/5
        [HttpPost, ActionName("EmployeesDelete")]
        [AuthorizeRoles(RoleNames.CustomerRole)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EmployeesDeleteConfirmed(int employeeID, int contractorID)
        {
            await employeeRepository.DeleteAsync(employeeID);
            //return RedirectToAction("Index");
            return RedirectToAction("Employees", "Contractors", new { id = contractorID });
        }

        #endregion

        #region " Vehicle Region "

        // GET: Contractors/Vehicles/5
        [AuthorizeRoles(RoleNames.CustomerRole)]
        public async Task<ActionResult> Vehicles(int id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            Contractor contractor = await contractorRepository.FindContractorsByIDAsync(id);
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

        // GET: Contractors/VehicleEdit
        [AuthorizeRoles(RoleNames.CustomerRole)]
        public async Task<ActionResult> VehiclesEdit(int vehicleID)
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
        [AuthorizeRoles(RoleNames.CustomerRole)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VehiclesEdit(EditContractorVehiclesViewModel model)
        {
            if (ModelState.IsValid)
            {
                Vehicle vehicle = model.GetVehicle();
                try
                {
                    vehicle.ModifiedDate = DateTime.UtcNow;
                    vehicle.ModifiedUser = System.Web.HttpContext.Current.User.Identity.Name;
                    await vehicleRepository.UpdateAsync(vehicle);

                    return RedirectToAction("Vehicles", "Contractors", new { id = model.ContractorID });
                }
                catch (Exception e)
                {
                    var errors = string.Join(",", e.Message);
                    ModelState.AddModelError(string.Empty, errors);
                }
            }

            return View(model);
        }

        // GET: Contractors/VehicleCreate
        [AuthorizeRoles(RoleNames.CustomerRole)]
        public ActionResult VehiclesCreate(int contractorID)
        {
            EditContractorVehiclesViewModel model = new EditContractorVehiclesViewModel();
            model.ContractorID = contractorID;
            return View(model);
        }

        [HttpPost]
        [AuthorizeRoles(RoleNames.CustomerRole)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VehiclesCreate(EditContractorVehiclesViewModel model)
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

                    return RedirectToAction("Vehicles", "Contractors", new { id = model.ContractorID });
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

        // GET: Contractors/VehiclesDelete
        [AuthorizeRoles(RoleNames.CustomerRole)]
        public async Task<ActionResult> VehiclesDelete(int vehicleID, int contractorID)
        {
            Vehicle vehicle = await vehicleRepository.FindVehicleByIDAsync(vehicleID);
            if (vehicle == null)
            {
                return HttpNotFound();
            }
            var model = new EditContractorVehiclesViewModel(vehicle);
            return View(model);
        }

        // POST: Contractors/VehiclesDeleteConfirmed/5
        [HttpPost, ActionName("VehiclesDelete")]
        [AuthorizeRoles(RoleNames.CustomerRole)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VehiclesDeleteConfirmed(int vehicleID, int contractorID)
        {
            await vehicleRepository.DeleteAsync(vehicleID);
            //return RedirectToAction("Index");
            return RedirectToAction("Vehicles", "Contractors", new { id = contractorID });
        }

        #endregion

        #region " Contract Region "

        // GET: Contractors/Contracts/5
        [AuthorizeRoles(RoleNames.CustomerRole)]
        public async Task<ActionResult> Contracts(int id)
        {
            Contractor contractor = await contractorRepository.FindContractorsByIDAsync(id);
            if (contractor == null)
            {
                return HttpNotFound();
            }

            if (contractor.CustomerID != base.CurrentCustomerID)
            {
                return HttpNotFound();
            }

            var model = new ContractorContractsViewModel(contractor);
            return View(model);
        }

        // GET: Contractors/ContractEdit
        [AuthorizeRoles(RoleNames.CustomerRole)]
        public async Task<ActionResult> ContractsEdit(int contractID)
        {
            Contract contract = await contractRepository.FindContractByIDAsync(contractID);
            if (contract == null)
            {
                return HttpNotFound();
            }
            var model = new EditContractorContractsViewModel(contract);
            return View(model);
        }

        [HttpPost]
        [AuthorizeRoles(RoleNames.CustomerRole)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ContractsEdit(EditContractorContractsViewModel model)
        {
            if (ModelState.IsValid)
            {
                Contract contract = model.GetContract();
                try
                {
                    //Contract.ModifiedDate = DateTime.UtcNow;
                    //Contract.ModifiedUser = System.Web.HttpContext.Current.User.Identity.Name;
                    await contractRepository.UpdateAsync(contract);

                    return RedirectToAction("Contracts", "Contractors", new { id = model.ContractorID });
                }
                catch (Exception e)
                {
                    var errors = string.Join(",", e.Message);
                    ModelState.AddModelError(string.Empty, errors);
                }
            }

            return View(model);
        }

        // GET: Contractors/VehicleCreate
        [AuthorizeRoles(RoleNames.CustomerRole)]
        public ActionResult ContractsCreate(int contractorID, int customerID)
        {
            EditContractorContractsViewModel model = new EditContractorContractsViewModel();
            model.ContractorID = contractorID;
            model.CustomerID = customerID;
            return View(model);
        }

        [HttpPost]
        [AuthorizeRoles(RoleNames.CustomerRole)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ContractsCreate(EditContractorContractsViewModel model)
        {

            if (ModelState.IsValid)
            {
                Contract contract = (Contract)model.GetContract();
                contract.ContractorID = model.ContractorID;
                contract.CustomerID = model.CustomerID;
                try
                {
                    //contract.CreationDate = DateTime.UtcNow;
                    //contract.CreationUser = System.Web.HttpContext.Current.User.Identity.Name;
                    await contractRepository.CreateAsync(contract);

                    return RedirectToAction("Contracts", "Contractors", new { id = model.ContractorID });
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

        // GET: Contractors/ContractsDelete
        [AuthorizeRoles(RoleNames.CustomerRole)]
        public async Task<ActionResult> ContractsDelete(int contractID, int contractorID)
        {
            Contract contract = await contractRepository.FindContractByIDAsync(contractID);
            if (contract == null)
            {
                return HttpNotFound();
            }
            var model = new EditContractorContractsViewModel(contract);
            return View(model);
        }

        // POST: Contractors/ContractsDeleteConfirmed/5
        [HttpPost, ActionName("ContractsDelete")]
        [AuthorizeRoles(RoleNames.CustomerRole)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ContractsDeleteConfirmed(int contractID, int contractorID)
        {
            await contractRepository.DeleteAsync(contractID);
            //return RedirectToAction("Index");
            return RedirectToAction("Contracts", "Contractors", new { id = contractorID });
        }

        #endregion

        #region " Assigned Employee Region "

        // GET: Contractors/AssignedEmployees/5
        [AuthorizeRoles(RoleNames.CustomerRole)]
        public async Task<ActionResult> AssignedEmployees(int contractID)
        {
            var employeesContracts = await contractRepository.FindEmployeeContractsAsync(contractID);

            Contract contract = await contractRepository.FindContractByIDAsync(contractID);
            if (contract == null)
            {
                return HttpNotFound();
            }

            var viewModel = new AssignedEmployeesViewModel(contract, employeesContracts);

            return View(viewModel);

        }

        // GET: BusinessTypes/EmployeesToAssign
        [AuthorizeRoles(RoleNames.CustomerRole)]
        public ActionResult EmployeesToAssign(int contractID, int contractorID)
        {
            EditAssignEmployeeViewModel model = new EditAssignEmployeeViewModel();
            model.ContractID = contractID;
            ViewBag.EmployeeID = new SelectList(employeeRepository.UnAssignedEmployees(contractID, contractorID), "EmployeeID", "FullName");

            return View(model);
        }

        [HttpPost]
        [AuthorizeRoles(RoleNames.CustomerRole)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EmployeesToAssign(EditAssignEmployeeViewModel model)
        {

            if (ModelState.IsValid)
            {
                EmployeeContract contract = (EmployeeContract)model.GetContract();

                try
                {
                    await employeeRepository.CreateContractAsync(contract);

                    return RedirectToAction("AssignedEmployees", "Contractors", new { contractID = model.ContractID });
                }
                catch (Exception e)
                {
                    var errors = string.Join(",", e.Message);
                    ModelState.AddModelError(string.Empty, errors);
                }
            }

            ViewBag.EmployeeID = new SelectList(employeeRepository.UnAssignedEmployees(model.ContractID, base.CurrentContractorID), "EmployeeID", "FullName");
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // GET: Contractors/EmployeesToUnassign
        [AuthorizeRoles(RoleNames.CustomerRole)]
        public async Task<ActionResult> EmployeesToUnassign(int employeeID, int contractID)
        {
            EmployeeContract employeeContract = await employeeRepository.GetEmployeeContract(employeeID, contractID);

            if (employeeContract == null)
            {
                return HttpNotFound();
            }
            var model = new EmployeeContractViewModel(employeeContract);
            return View(model);
        }

        //POST: Contractors/EmployeesToUnassign/5
        [HttpPost, ActionName("EmployeesToUnassign")]
        [AuthorizeRoles(RoleNames.CustomerRole)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EmployeesToUnassignConfirmed(int employeeID, int contractID)
        {
            await employeeRepository.DeleteContract(employeeID, contractID);

            return RedirectToAction("AssignedEmployees", "Contractors", new { contractID = contractID });
        }

        #endregion


        #region " Assigned Vehicles Region "

        // GET: Contractors/AssignedVehicles/5
        [AuthorizeRoles(RoleNames.CustomerRole)]
        public async Task<ActionResult> AssignedVehicles(int contractID)
        {
            var VehiclesContracts = await contractRepository.FindVehicleContractsAsync(contractID);

            Contract contract = await contractRepository.FindContractByIDAsync(contractID);
            if (contract == null)
            {
                return HttpNotFound();
            }

            var viewModel = new AssignedVehiclesViewModel(contract, VehiclesContracts);

            return View(viewModel);

        }

        // GET: BusinessTypes/VehiclesToAssign
        [AuthorizeRoles(RoleNames.CustomerRole)]
        public ActionResult VehiclesToAssign(int contractID, int contractorID)
        {
            EditAssignVehicleViewModel model = new EditAssignVehicleViewModel();
            model.ContractID = contractID;
            ViewBag.VehicleID = new SelectList(vehicleRepository.UnAssignedVehicles(contractID, contractorID), "VehicleID", "IdentificationNumber");

            return View(model);
        }

        [HttpPost]
        [AuthorizeRoles(RoleNames.CustomerRole)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VehiclesToAssign(EditAssignVehicleViewModel model)
        {

            if (ModelState.IsValid)
            {
                VehicleContract contract = (VehicleContract)model.GetContract();

                try
                {
                    await vehicleRepository.CreateContractAsync(contract);

                    return RedirectToAction("AssignedVehicles", "Contractors", new { contractID = model.ContractID });
                }
                catch (Exception e)
                {
                    var errors = string.Join(",", e.Message);
                    ModelState.AddModelError(string.Empty, errors);
                }
            }

            ViewBag.VehicleID = new SelectList(vehicleRepository.UnAssignedVehicles(model.ContractID, base.CurrentContractorID), "VehicleID", "IdentificationNumber");
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // GET: Contractors/VehiclesToUnassign
        [AuthorizeRoles(RoleNames.CustomerRole)]
        public async Task<ActionResult> VehiclesToUnassign(int vehicleID, int contractID)
        {
            VehicleContract vehicleContract = await vehicleRepository.GetVehicleContract(vehicleID, contractID);

            if (vehicleContract == null)
            {
                return HttpNotFound();
            }
            var model = new VehicleContractViewModel(vehicleContract);
            return View(model);
        }

        //POST: Contractors/VehiclesToUnassign/5
        [HttpPost, ActionName("VehiclesToUnassign")]
        [AuthorizeRoles(RoleNames.CustomerRole)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VehiclesToUnassignConfirmed(int vehicleID, int contractID)
        {
            await vehicleRepository.DeleteContract(vehicleID, contractID);

            return RedirectToAction("AssignedVehicles", "Contractors", new { contractID = contractID });
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (contractorRepository != null)
                {
                    contractorRepository.Dispose();
                    contractorRepository = null;
                }
                
                if (localizationRepository != null)
                {
                    localizationRepository.Dispose();
                    localizationRepository = null;
                }
                
                if (businessTypeRepository != null)
                {
                    businessTypeRepository.Dispose();
                    businessTypeRepository = null;
                }
                
                if (employeeRepository != null)
                {
                    employeeRepository.Dispose();
                    employeeRepository = null;
                }
                
                if (vehicleRepository != null)
                {
                    vehicleRepository.Dispose();
                    vehicleRepository = null;
                }
                
                if (employeeRelationshipTypeRepository != null)
                {
                    employeeRelationshipTypeRepository.Dispose();
                    employeeRelationshipTypeRepository = null;
                }
                
                if (userManager != null)
                {
                    userManager.Dispose();
                    userManager = null;
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
