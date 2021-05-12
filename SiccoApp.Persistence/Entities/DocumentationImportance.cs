using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SiccoApp.Persistence
{
    public class DocumentationImportance : BaseEntity
    {
        public int DocumentationImportanceID { get; set; }

        [Display(Name = "Importancia")]
        public string Description { get; set; }
    }
}
