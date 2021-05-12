using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Resources;

namespace SiccoApp.Persistence
{
    public class EntityType
    {
        public int EntityTypeID { get; set; }

        [Display(Name = "Tipo Entidad")]
        public string Description { get; set; }
    }
}
