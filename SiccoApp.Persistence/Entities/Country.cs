using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiccoApp.Persistence
{
    public class Country
    {
        public int CountryID { get; set; }
        public string CountryName { get; set; }

        public ICollection<State> States { get; set; }
    }
}
