using System;
using System.ComponentModel.DataAnnotations;

namespace SiccoApp.Persistence
{
    public class DocumentationTemplateMetaData
    {
        [Display(Name = "CodDocumentationTemplateName", ResourceType = typeof(Resources.Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "CodDocumentationTemplateRequired")]
        [StringLength(20, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "CodDocumentationTemplateLong")]
        public string DocumentationTemplateCode { get; set; }
        //------------------------------------------------------------------------------------------------------------

        [Display(Name = "DescDocumentationTemplateName", ResourceType = typeof(Resources.Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "DescDocumentationTemplateRequired")]
        [StringLength(150, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "DescDocumentationTemplateLong")]
        public string Description { get; set; }
        //------------------------------------------------------------------------------------------------------------

        [Display(Name = "DocEntityType", ResourceType = typeof(Resources.Resources))]
        public int EntityTypeID { get; set; }
        //------------------------------------------------------------------------------------------------------------

    }
    public static class DocumentationTemplateRules
    {
        public static ValidationResult ValidateDocumentationTemplate(DocumentationTemplate documentationTemplate, ValidationContext validationContext)
        {
            if (documentationTemplate.Description == "noononononono")
            {
                return new ValidationResult("Regla boba 1 para test en Tax");
            }

            return null;
        }
    }
}
