using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//http://www.scriptscoop2.com/t/62a42c164375/c-creating-inheritance-users-from-base-asp-net-identity-user.html

namespace SiccoApp.Persistence
{
    public class CustomerUser : ApplicationUser
    {
        public int CustomerID { get; set; }
    }
}
