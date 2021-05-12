using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace SiccoApp.Persistence
{
    [MetadataType(typeof(BusinessTypeMetaData))]
    public class BusinessType
    {
        public int BusinessTypeID { get; set; }
        public string BusinessTypeCode { get; set; }
        public string Description { get; set; }

        public ICollection<Contractor> Contractor { get; set; }
        public ICollection<DocumentationBusinessType> DocumentationBusinessType { get; set; }

        public int CustomerID { get; set; }
        public Customer Customer { get; set; }
    }
}
