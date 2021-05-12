using System.ComponentModel.DataAnnotations;

namespace SiccoApp.Persistence
{
    public class BusinessTypeMetaData
    {
        [Required]
        [StringLength(20)]
        [Display(Name = "CodBusinessTypeName", ResourceType = typeof(Resources.Resources))]
        public string BusinessTypeCode { get; set; }

        [StringLength(150)]
        [Display(Name = "DescBusinessTypeName", ResourceType = typeof(Resources.Resources))]
        public string Description { get; set; }
    }
}
