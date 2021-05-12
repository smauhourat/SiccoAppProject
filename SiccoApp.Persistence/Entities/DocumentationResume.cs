using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiccoApp.Persistence
{
    [Table("vwDocumentations")]
    public class DocumentationResume
    {
        public Guid Id { get; set; }
        public int CustomerID { get; set; }
        public int ContractorID { get; set; }
        public string BusinessTypeCode{ get; set; }
        public string BusinessTypeDescription { get; set; }
        public string DocumentationCode { get; set; }
        public string DocumentationDescription { get; set; }
        public string EntityTypeDescription { get; set; }
        public string DocumentationPeriodicityDescription { get; set; }

    }
}
