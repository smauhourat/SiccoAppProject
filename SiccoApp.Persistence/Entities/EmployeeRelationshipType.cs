using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace SiccoApp.Persistence
{
    [MetadataType(typeof(EmployeeRelationshipTypeMetaData))]
    public class EmployeeRelationshipType
    {
        public int EmployeeRelationshipTypeID { get; set; }
        public string Description { get; set; }
    }
}
