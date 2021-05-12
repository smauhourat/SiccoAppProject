using SiccoApp.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiccoApp.Models
{
    public class ContractorDashboardViewModel
    {
        public int ContractorID { get; set; }
        public string CompanyName { get; set; }
        public int TotalQuantityEmployees { get; set; }
        public int TotalQuantityVehicles { get; set; }

#warning "Mejorar la cuenta de empleados y vehiculos"
        public ContractorDashboardViewModel(Contractor contractor)
        {
            this.ContractorID = contractor.ContractorID;
            this.CompanyName = contractor.CompanyName;
            this.TotalQuantityEmployees = contractor.Employees.Count();
            this.TotalQuantityVehicles = contractor.Vehicles.Count();
        }
    }
}