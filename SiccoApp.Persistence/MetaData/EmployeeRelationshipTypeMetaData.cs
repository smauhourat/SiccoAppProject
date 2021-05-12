using System.ComponentModel.DataAnnotations;

namespace SiccoApp.Persistence
{
    public class EmployeeRelationshipTypeMetaData
    {
        [Required]
        public string EmployeeRelationshipTypeID { get; set; }

        [StringLength(50)]
        [Display(Name = "EmployeeRelationshipType", ResourceType = typeof(Resources.Resources))]
        public string Description { get; set; }
    }
}
