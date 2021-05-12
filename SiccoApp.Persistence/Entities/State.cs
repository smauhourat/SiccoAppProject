﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiccoApp.Persistence
{
    public class State
    {
        public int StateID { get; set; }
        public string StateName { get; set; }
        public int CountryID { get; set; }

        public ICollection<Customer> Customers { get; set; }
    }
}
