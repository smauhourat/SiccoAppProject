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
    
    public class BusinessTypeTemplate
    {
        public int BusinessTypeTemplateID { get; set; }

        [Display(Name = "CodBusinessTypeName", ResourceType = typeof(Resources.Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "CodBusinessTypeRequired")]
        [StringLength(20, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "CodBusinessTypeLong")]
        public string BusinessTypeTemplateCode { get; set; }
        //------------------------------------------------------------------------------------------------------------

        [Display(Name = "DescBusinessTypeName", ResourceType = typeof(Resources.Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "DescBusinessTypeRequired")]
        [StringLength(150, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "DescBusinessTypeLong")]
        public string Description { get; set; }
        //------------------------------------------------------------------------------------------------------------    

        public virtual ICollection<DocumentationBusinessTypeTemplate> DocumentationBusinessTypeTemplate { get; set; }
        //------------------------------------------------------------------------------------------------------------
    }
}
