using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using SiccoApp.Persistence;

namespace SiccoApp.Models
{


    public class RequirementViewModel
    {
        public int RequirementID { get; set; }

        public string Semaphore { get; set; }

        public string ContractCode { get; set; }

        [Display(Name = "CodDocumentationName", ResourceType = typeof(Resources.Resources))]
        public string DocumentationCode { get; set; }

        [Display(Name = "DescDocumentationName", ResourceType = typeof(Resources.Resources))]
        public string Description { get; set; }

        //identificacion del contratista, empleado o vehiculo
        [Display(Name = "ResourceIdentification", ResourceType = typeof(Resources.Resources))]
        public string ResourceIdentification { get; set; }

        [Display(Name = "ResourceType", ResourceType = typeof(Resources.Resources))]
        public string ResourceType { get; set; }

        public int EntityTypeID { get; set; }

        [Display(Name = "PeriodID", ResourceType = typeof(Resources.Resources))]
        public int PeriodID { get; set; }

        [Display(Name = "RequirementStatus", ResourceType = typeof(Resources.Resources))]
        public RequirementStatus RequirementStatus { get; set; }

        [Display(Name = "DueDate", ResourceType = typeof(Resources.Resources))]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DueDate { get; set; }
        [Display(Name = "PresentationCount", ResourceType = typeof(Resources.Resources))]
        public int PresentationCount { get; set; }

        public RequirementViewModel(Requirement requirement)
        {
            this.RequirementID = requirement.RequirementID;

            switch (requirement.RequirementStatus)
            {
                case RequirementStatus.Pending:
                    this.Semaphore = "red";
                    break;
                case RequirementStatus.ToProcess:
                    this.Semaphore = "yellow";
                    break;
                case RequirementStatus.Processing:
                    this.Semaphore = "yellow";
                    break;
                case RequirementStatus.Approved:
                    this.Semaphore = "green";
                    break;
                case RequirementStatus.Rejected:
                    this.Semaphore = "red";
                    break;
            }

            this.ContractCode = requirement.Contract.ContractCode;
            this.DocumentationCode = requirement.DocumentationBusinessType.Documentation.DocumentationCode;
            this.Description = requirement.DocumentationBusinessType.Documentation.Description;

            //Conrtratista
            if (requirement.EmployeeContract == null && requirement.VehicleContract == null)
            {
                this.ResourceType = ResourceTypeViewModel.Contractor;
                this.ResourceIdentification = requirement.Contract.Contractor.CompanyName;
            }

            //Empleado
            if (requirement.EmployeeContract != null)
            {
                this.ResourceType = ResourceTypeViewModel.Employee;
                this.ResourceIdentification = requirement.EmployeeContract.Employee.LastName + ", " + requirement.EmployeeContract.Employee.FirstName;
            }

            //Vehiculo
            if (requirement.VehicleContract != null)
            {
                this.ResourceType = ResourceTypeViewModel.Vehicle;
                this.ResourceIdentification = requirement.VehicleContract.Vehicle.IdentificationNumber;
            }

            this.PeriodID = requirement.PeriodID;
            this.RequirementStatus = requirement.RequirementStatus;
            this.DueDate = requirement.DueDate;

            this.PresentationCount = requirement.Presentations.Count;

            this.EntityTypeID = requirement.DocumentationBusinessType.Documentation.EntityTypeID;
        }
    }

    public static class RequirementViewModelExtensions
    {
        public static RequirementViewModel ToDisplayViewModel(this Requirement source)
        {
            return new RequirementViewModel(source);
        }
    }

}
