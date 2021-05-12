using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace SiccoApp.Persistence
{

    public enum ContractStatus
    {
        [Description("Active")]
        Active = 1,

        [Description("Complete")]
        Complete = 2,

        [Description("StandBy")]
        StandBy = 3,

        [Description("Close")]
        Close = 4
    }

    public class Contract
    {
        public int ContractID { get; set; }
        public int ContractorID { get; set; }
        public int CustomerID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string ContractCode { get; set; }

        public string Description { get; set; }

        public ContractStatus ContractStatusID { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> StartDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<int> Score { get; set; }
    
        public virtual Contractor Contractor { get; set; }
        public Customer Customer { get; set; }

        //public virtual ICollection<EmployeeContract> EmployeeContract { get; set; }
        //public virtual ICollection<VehicleContract> VehicleContract { get; set; }
    }
}
