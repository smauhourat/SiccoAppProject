using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SiccoApp.Persistence
{
    public class Documentation
    {
        public int DocumentationID { get; set; }

        [Display(Name = "CodDocumentationName", ResourceType = typeof(Resources.Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "CodDocumentationRequired")]
        [StringLength(20, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "CodDocumentationLong")]
        public string DocumentationCode { get; set; }
        //------------------------------------------------------------------------------------------------------------

        [Display(Name = "DescDocumentationName", ResourceType = typeof(Resources.Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "DescDocumentationRequired")]
        [StringLength(150, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "DescDocumentationLong")]
        public string Description { get; set; }
        //------------------------------------------------------------------------------------------------------------

        [Display(Name = "DocEntityType", ResourceType = typeof(Resources.Resources))]
        public int EntityTypeID { get; set; }
        public virtual EntityType EntityType { get; set; }
        //------------------------------------------------------------------------------------------------------------

        public ICollection<DocumentationBusinessType> DocumentationBusinessType { get; set; }
        //------------------------------------------------------------------------------------------------------------

        public int CustomerID { get; set; }
        public Customer Customer { get; set; }
        //------------------------------------------------------------------------------------------------------------

    }
}
