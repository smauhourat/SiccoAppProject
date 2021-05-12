using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;
using Resources;

namespace SiccoApp.Persistence
{
    [CustomValidation(typeof(DocumentationTemplateRules), "ValidateDocumentationTemplate")]
    [MetadataType(typeof(DocumentationTemplateMetaData))]
    public class DocumentationTemplate //: IValidatableObject
    {
        public int DocumentationTemplateID { get; set; }
        //------------------------------------------------------------------------------------------------------------

        public string DocumentationTemplateCode { get; set; }
        //------------------------------------------------------------------------------------------------------------

        public string Description { get; set; }
        //------------------------------------------------------------------------------------------------------------
        public int EntityTypeID { get; set; }
        public virtual EntityType EntityType { get; set; }
        //------------------------------------------------------------------------------------------------------------

        public virtual ICollection<DocumentationBusinessTypeTemplate> DocumentationBusinessTypeTemplate { get; set; }
        //------------------------------------------------------------------------------------------------------------
    }
}
