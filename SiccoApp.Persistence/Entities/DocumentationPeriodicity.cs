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
    
    public class DocumentationPeriodicity
    {
        public int DocumentationPeriodicityID { get; set; }

        [Display(Name = "Periodicidad")]
        public string Description { get; set; }

        //public ICollection<Documentation> Documentation { get; set; }
        //public ICollection<DocumentationTemplate> DocumentationTemplate { get; set; }
        public ICollection<DocumentationBusinessType> DocumentationBusinessType { get; set; }
        public ICollection<DocumentationBusinessTypeTemplate> DocumentationBusinessTypeTemplate { get; set; }
    }
}
