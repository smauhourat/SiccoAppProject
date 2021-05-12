using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiccoApp.Persistence
{
    public class DocumentationBusinessType
    {
        public int DocumentationBusinessTypeID { get; set; }
        //------------------------------------------------------------------------------------------------------------

        public int DocumentationID { get; set; }
        //------------------------------------------------------------------------------------------------------------

        public int BusinessTypeID { get; set; }
        //------------------------------------------------------------------------------------------------------------

        public int DocumentationImportanceID { get; set; }
        public DocumentationImportance DocumentationImportance { get; set; }
        //------------------------------------------------------------------------------------------------------------

        [Display(Name = "Periodicity", ResourceType = typeof(Resources.Resources))]
        public Nullable<int> DocumentationPeriodicityID { get; set; }
        public virtual DocumentationPeriodicity DocumentationPeriodicity { get; set; }
        //------------------------------------------------------------------------------------------------------------

        [Display(Name = "RestrictAccess", ResourceType = typeof(Resources.Resources))]
        public bool RestrictAccess { get; set; }
        //------------------------------------------------------------------------------------------------------------

        public virtual BusinessType BusinessType { get; set; }
        //------------------------------------------------------------------------------------------------------------

        public virtual Documentation Documentation { get; set; }
        //------------------------------------------------------------------------------------------------------------
    }
}
