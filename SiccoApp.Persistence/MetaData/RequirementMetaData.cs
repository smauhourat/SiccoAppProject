using System;
using System.ComponentModel.DataAnnotations;

namespace SiccoApp.Persistence
{
    public class RequirementMetaData
    {
        [Display(Name = "PeriodID", ResourceType = typeof(Resources.Resources))]
        public int PeriodID { get; set; }

        [Display(Name = "RequirementStatus", ResourceType = typeof(Resources.Resources))]
        public RequirementStatus RequirementStatus { get; set; }

        [Display(Name = "DueDate", ResourceType = typeof(Resources.Resources))]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DueDate { get; set; }
    }
}