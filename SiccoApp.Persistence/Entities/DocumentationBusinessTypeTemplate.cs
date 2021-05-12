using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SiccoApp.Persistence
{
    
    public class DocumentationBusinessTypeTemplate
    {
        public int DocumentationBusinessTypeTemplateID { get; set; }
        //------------------------------------------------------------------------------------------------------------

        public int DocumentationTemplateID { get; set; }
        //------------------------------------------------------------------------------------------------------------

        public int BusinessTypeTemplateID { get; set; }
        //------------------------------------------------------------------------------------------------------------

        public int DocumentationImportanceID { get; set; }
        public DocumentationImportance DocumentationImportance { get; set; }
        //------------------------------------------------------------------------------------------------------------

        [Display(Name = "Periodicity", ResourceType = typeof(Resources.Resources))]
        public int DocumentationPeriodicityID { get; set; }
        public virtual DocumentationPeriodicity DocumentationPeriodicity { get; set; }
        //------------------------------------------------------------------------------------------------------------

        [Display(Name = "RestrictAccess", ResourceType = typeof(Resources.Resources))]
        public bool RestrictAccess { get; set; }
        //------------------------------------------------------------------------------------------------------------

        public virtual BusinessTypeTemplate BusinessTypeTemplate { get; set; }
        //------------------------------------------------------------------------------------------------------------

        public virtual DocumentationTemplate DocumentationTemplate { get; set; }
        //------------------------------------------------------------------------------------------------------------
    }
}
