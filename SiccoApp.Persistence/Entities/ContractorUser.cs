using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiccoApp.Persistence
{
    public class ContractorUser: ApplicationUser, IUser
    {
        public int ContractorID { get; set; }
        //public virtual Contractor contractor { get; set; }
    }
}
