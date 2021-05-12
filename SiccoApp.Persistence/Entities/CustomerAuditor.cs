using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace SiccoApp.Persistence
{
    public class CustomerAuditor
    {
        //public int? CustomerAuditorID { get; set; }
        //public int CustomerID { get; set; }
        //public string UserId { get; set; }
        //public virtual AdminUser User { get; set; }

        //public CustomerAuditor() { }
        //public CustomerAuditor(int? customerAuditorID, int customerID, string userId)
        //{
        //    CustomerAuditorID = customerAuditorID;
        //    CustomerID = customerID;
        //    UserId = userId;
        //}

        public int? CustomerAuditorID { get; set; }
        public int CustomerID { get; set; }
        public string UserId { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual AdminUser User { get; set; }

        public CustomerAuditor() { }
        public CustomerAuditor(int? customerAuditorID, int customerID, string userId)
        {
            CustomerAuditorID = customerAuditorID;
            CustomerID = customerID;
            UserId = userId;
        }
    }
}
