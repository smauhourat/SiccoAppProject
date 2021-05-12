using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using SiccoApp.Persistence;
using System;

namespace SiccoApp.Models
{
    public class AssignedDocumentTemplatesViewModel
    {
        public int BussinessTypeTemplateID { get; set; }
        public string BussinessTypeTemplateDescription { get; set; }
        public IEnumerable<DocumentationBusinessTypeTemplate> DocumentationBusinessTypeTemplates { get; set; }

        public AssignedDocumentTemplatesViewModel(BusinessTypeTemplate businessTypeTemplate, List<DocumentationBusinessTypeTemplate> documentationBusinessTypeTemplates)
        {
            this.BussinessTypeTemplateID = businessTypeTemplate.BusinessTypeTemplateID;
            this.BussinessTypeTemplateDescription = businessTypeTemplate.Description;
            this.DocumentationBusinessTypeTemplates = documentationBusinessTypeTemplates;
        }
    }

    public class EditAssignDocumentTemplateViewModel
    {
        public int DocumentationBusinessTypeTemplateID { get; set; }
        //------------------------------------------------------------------------------------------------------------

        [Display(Name = "Documentation", ResourceType = typeof(Resources.Resources))]
        public int DocumentationTemplateID { get; set; }
        //------------------------------------------------------------------------------------------------------------

        public int BusinessTypeTemplateID { get; set; }
        //------------------------------------------------------------------------------------------------------------

        public int DocumentationImportanceID { get; set; }
        //------------------------------------------------------------------------------------------------------------

        [Display(Name = "Documentation", ResourceType = typeof(Resources.Resources))]
        public string DocumentationTemplateDesc { get; set; }
        //------------------------------------------------------------------------------------------------------------

        [Display(Name = "Periodicity", ResourceType = typeof(Resources.Resources))]
        public int DocumentationPeriodicityID { get; set; }
        public DocumentationPeriodicity DocumentationPeriodicity { get; set; }
        //------------------------------------------------------------------------------------------------------------

        [Display(Name = "RestrictAccess", ResourceType = typeof(Resources.Resources))]
        public Boolean RestrictAccess { get; set; }
        //------------------------------------------------------------------------------------------------------------

        public EditAssignDocumentTemplateViewModel() { }

        public EditAssignDocumentTemplateViewModel(DocumentationBusinessTypeTemplate docuBusinessType)
        {
            this.DocumentationBusinessTypeTemplateID = docuBusinessType.DocumentationBusinessTypeTemplateID;
            this.DocumentationTemplateID = docuBusinessType.DocumentationTemplateID;
            this.BusinessTypeTemplateID = docuBusinessType.BusinessTypeTemplateID;
            this.DocumentationImportanceID = docuBusinessType.DocumentationImportanceID;
            this.DocumentationTemplateDesc = docuBusinessType.DocumentationTemplate.DocumentationTemplateCode + " - (" + docuBusinessType.DocumentationTemplate.Description + ")";
            this.DocumentationPeriodicityID = docuBusinessType.DocumentationPeriodicityID;
            this.DocumentationPeriodicity = docuBusinessType.DocumentationPeriodicity;
            this.RestrictAccess = docuBusinessType.RestrictAccess;
        }

        public DocumentationBusinessTypeTemplate GetDocument()
        {
            var document = new DocumentationBusinessTypeTemplate()
            {
                DocumentationBusinessTypeTemplateID = this.DocumentationBusinessTypeTemplateID,
                DocumentationTemplateID = this.DocumentationTemplateID,
                BusinessTypeTemplateID = this.BusinessTypeTemplateID,
                DocumentationImportanceID = this.DocumentationImportanceID,
                DocumentationPeriodicityID = this.DocumentationPeriodicityID,
                RestrictAccess = this.RestrictAccess
            };
            return document;
        }
    }
}