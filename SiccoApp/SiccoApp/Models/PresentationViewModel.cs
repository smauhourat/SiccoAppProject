using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using SiccoApp.Persistence;

namespace SiccoApp.Models
{
    public class PresentationViewModel
    {
        public int PresentationID { get; set; }

        public string Semaphore { get; set; }

        public int RequirementID { get; set; }

        [Display(Name = "CodDocumentationName", ResourceType = typeof(Resources.Resources))]
        public string DocumentationCode { get; set; }

        [Display(Name = "DescDocumentationName", ResourceType = typeof(Resources.Resources))]
        public string DocumentationDescription { get; set; }



        //identificacion del contratista, empleado o vehiculo
        [Display(Name = "ResourceIdentification", ResourceType = typeof(Resources.Resources))]
        public string ResourceIdentification { get; set; }

        [Display(Name = "ResourceType", ResourceType = typeof(Resources.Resources))]
        public string ResourceType { get; set; }



        [Display(Name = "PresentationState", ResourceType = typeof(Resources.Resources))]
        public PresentationStatus PresentationStatus { get; set; }

        [Display(Name = "PresentationDate", ResourceType = typeof(Resources.Resources))]
        //[DataType(DataType.DateTime), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.DateTime), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm}", ApplyFormatInEditMode = true)]
        public System.DateTime PresentationDate { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "DocumentAttach_Required")]
        [Display(Name = "DocumentFiles", ResourceType = typeof(Resources.Resources))]
        public string DocumentFiles { get; set; }

        public string TakenForID { get; set; }

        [Display(Name = "TakenFor", ResourceType = typeof(Resources.Resources))]
        public ApplicationUser TakenFor { get; set; }

        public string TakenForFullName
        {
            get
            {
                return (this.TakenFor != null) ? this.TakenFor.LastName + ", " + this.TakenFor.FirstName : "";
            }
            set { }
        }

        [Display(Name = "TakenDate", ResourceType = typeof(Resources.Resources))]
        [DataType(DataType.DateTime), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> TakenDate { get; set; }

        public string ApprovedForID { get; set; }

        [Display(Name = "ApprovedFor", ResourceType = typeof(Resources.Resources))]
        public ApplicationUser ApprovedFor { get; set; }

        public string ApprovedForFullName
        {
            get
            {
                return (this.ApprovedFor != null) ? this.ApprovedFor.LastName + ", " + this.ApprovedFor.FirstName : "";
            }
            set { }
        }

        [Display(Name = "ApprovedDate", ResourceType = typeof(Resources.Resources))]
        [DataType(DataType.DateTime), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> ApprovedDate { get; set; }

        public string RejectedForID { get; set; }

        [Display(Name = "RejectedDate", ResourceType = typeof(Resources.Resources))]
        [DataType(DataType.DateTime), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> RejectedDate { get; set; }

        //[Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "DocumentAttach_Required")]
        [Display(Name = "Observations", ResourceType = typeof(Resources.Resources))]
        public string Observations { get; set; }

        [Display(Name = "RequirementStatus", ResourceType = typeof(Resources.Resources))]
        public RequirementStatus RequirementStatus { get; set; }

        public PresentationViewModel()
        {
        }

        public PresentationViewModel(Requirement requirement)
        {
            RequirementID = requirement.RequirementID;
            DocumentationCode = requirement.DocumentationBusinessType.Documentation.DocumentationCode;
            DocumentationDescription = requirement.DocumentationBusinessType.Documentation.Description;
            RequirementStatus = requirement.RequirementStatus;

            if (requirement.EmployeeContract == null && requirement.VehicleContract == null)
            {
                this.ResourceType = ResourceTypeViewModel.Contractor;
                this.ResourceIdentification = requirement.Contract.Contractor.CompanyName;
            }
            else
            {
                if (requirement.EmployeeContract == null)
                {
                    this.ResourceType = ResourceTypeViewModel.Vehicle;
                    this.ResourceIdentification = requirement.VehicleContract.Vehicle.IdentificationNumber;
                }
                else
                {
                    this.ResourceType = ResourceTypeViewModel.Employee;
                    this.ResourceIdentification = requirement.EmployeeContract.Employee.LastName + ", " + requirement.EmployeeContract.Employee.FirstName;
                }
            }
        }

        public PresentationViewModel(Presentation presentation)
        {
            PresentationID = presentation.PresentationID;

            switch (presentation.PresentationStatus)
            {
                case PresentationStatus.Pending:
                    this.Semaphore = "red";
                    break;
                case PresentationStatus.ToProccess:
                    this.Semaphore = "yellow";
                    break;
                case PresentationStatus.Processing:
                    this.Semaphore = "yellow";
                    break;
                case PresentationStatus.Approved:
                    this.Semaphore = "green";
                    break;
                case PresentationStatus.Rejected:
                    this.Semaphore = "red";
                    break;
            }

            RequirementID = presentation.RequirementID;
            DocumentationCode = presentation.Requirement.DocumentationBusinessType.Documentation.DocumentationCode;
            DocumentationDescription = presentation.Requirement.DocumentationBusinessType.Documentation.Description;
            PresentationStatus = presentation.PresentationStatus;
            PresentationDate = presentation.PresentationDate;
            DocumentFiles = presentation.DocumentFiles;
            TakenForID = presentation.TakenForID;
            TakenFor = presentation.TakenFor;
            TakenDate = presentation.TakenDate;
            ApprovedForID = presentation.ApprovedForID;
            ApprovedFor = presentation.ApprovedFor;
            ApprovedDate = presentation.ApprovedDate;
            RejectedForID = presentation.RejectedForID;
            RejectedDate = presentation.RejectedDate;
            Observations = presentation.Observations;

            if (presentation.Requirement.EmployeeContract == null && presentation.Requirement.VehicleContract == null)
            {
                this.ResourceType = ResourceTypeViewModel.Contractor;
                this.ResourceIdentification = presentation.Requirement.Contract.Contractor.CompanyName;
            }
            else
            {
                if (presentation.Requirement.EmployeeContract == null)
                {
                    this.ResourceType = ResourceTypeViewModel.Vehicle;
                    this.ResourceIdentification = presentation.Requirement.VehicleContract.Vehicle.IdentificationNumber;
                }
                else
                {
                    this.ResourceType = ResourceTypeViewModel.Employee;
                    this.ResourceIdentification = presentation.Requirement.EmployeeContract.Employee.LastName + ", " + presentation.Requirement.EmployeeContract.Employee.FirstName;
                }
            }
        }

        // Return a pre-poulated instance of Presentation:
        public Presentation GetPresentation()
        {
            var presentation = new Presentation()
            {
                PresentationID = this.PresentationID,
                RequirementID = this.RequirementID,

                PresentationStatus = this.PresentationStatus,
                DocumentFiles = this.DocumentFiles,

                TakenForID = this.TakenForID,
                TakenDate = this.TakenDate,

                ApprovedForID = this.ApprovedForID,
                ApprovedDate = this.ApprovedDate,

                RejectedForID = this.RejectedForID,
                RejectedDate = this.RejectedDate,

                Observations = this.Observations
            };
            return presentation;
        }
    }

    public static class PresentationViewModelExtensions
    {
        public static PresentationViewModel ToDisplayViewModel(this Presentation source)
        {
            return new PresentationViewModel(source);
        }
    }
}
