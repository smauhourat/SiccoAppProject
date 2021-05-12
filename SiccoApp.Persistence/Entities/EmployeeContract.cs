using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiccoApp.Persistence
{
    public class EmployeeContract
    {
        public int EmployeeContractID { get; set; }
        public int EmployeeID { get; set; }
        public int ContractID { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual Contract Contract { get; set; }
    }
}
