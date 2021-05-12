using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using SiccoApp.Persistence;
using System;

namespace SiccoApp.Models
{
    public class AssignedDocumentsViewModel
    {
        public int BussinessTypeID { get; set; }
        public string BussinessTypeDescription { get; set; }
        public IEnumerable<DocumentationBusinessType> DocumentationBusinessTypes { get; set; }

        public AssignedDocumentsViewModel(BusinessType businessType, List<DocumentationBusinessType> documentationBusinessTypes)
        {
            this.BussinessTypeID = businessType.BusinessTypeID;
            this.BussinessTypeDescription = businessType.Description;
            this.DocumentationBusinessTypes = documentationBusinessTypes;
        }
    }

    public class EditAssignDocumentViewModel
    {
        public int DocumentationBusinessTypeID { get; set; }
        //------------------------------------------------------------------------------------------------------------

        [Display(Name = "Documentation", ResourceType = typeof(Resources.Resources))]
        public int DocumentationID { get; set; }
        //------------------------------------------------------------------------------------------------------------

        public int BusinessTypeID { get; set; }
        //------------------------------------------------------------------------------------------------------------

        [Display(Name = "Importance", ResourceType = typeof(Resources.Resources))]
        //[Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "Importance_Required")]
        //[Range(1, 10, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "Importance_Range")]
        public byte Importance { get; set; }
        //------------------------------------------------------------------------------------------------------------

        [Display(Name = "Periodicity", ResourceType = typeof(Resources.Resources))]
        public Nullable<int> DocumentationPeriodicityID { get; set; }
        public virtual DocumentationPeriodicity DocumentationPeriodicity { get; set; }
        //------------------------------------------------------------------------------------------------------------

        [Display(Name = "RestrictAccess", ResourceType = typeof(Resources.Resources))]
        public Boolean RestrictAccess { get; set; }
        //------------------------------------------------------------------------------------------------------------

        [Display(Name = "Documentation", ResourceType = typeof(Resources.Resources))]
        public string DocumentationDesc { get; set; }
        //------------------------------------------------------------------------------------------------------------

        public EditAssignDocumentViewModel() { }

        public EditAssignDocumentViewModel(DocumentationBusinessType docuBusinessType)
        {
            this.DocumentationBusinessTypeID = docuBusinessType.DocumentationBusinessTypeID;
            this.DocumentationID = docuBusinessType.DocumentationID;
            this.BusinessTypeID = docuBusinessType.BusinessTypeID;
            this.DocumentationDesc = docuBusinessType.Documentation.DocumentationCode + " - (" + docuBusinessType.Documentation.Description + ")";
            this.DocumentationPeriodicityID = docuBusinessType.DocumentationPeriodicityID;
            this.DocumentationPeriodicity = docuBusinessType.DocumentationPeriodicity;
            this.RestrictAccess = docuBusinessType.RestrictAccess;
        }

        public DocumentationBusinessType GetDocument()
        {
            var document = new DocumentationBusinessType()
            {
                DocumentationBusinessTypeID = this.DocumentationBusinessTypeID,
                DocumentationID = this.DocumentationID,
                BusinessTypeID = this.BusinessTypeID,
                DocumentationPeriodicityID = this.DocumentationPeriodicityID,
                RestrictAccess = this.RestrictAccess
            };
            return document;
        }
    }
}