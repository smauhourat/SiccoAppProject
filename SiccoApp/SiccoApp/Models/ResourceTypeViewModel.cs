using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiccoApp.Models
{
    public static class ResourceTypeViewModel
    {
        public static string Contractor { get { return "CONTRATISTA"; } }
        public static string Employee { get { return "EMPLEADO"; } }
        public static string Vehicle { get { return "VEHICULO"; } }

        public static List<String> ToList()
        {
            return new List<string>() { ResourceTypeViewModel.Contractor, ResourceTypeViewModel.Employee, ResourceTypeViewModel.Vehicle };
        }
    }

    //public static class ResourceTypeViewModelExtensions
    //{
    //    public static List<String> ToList()(this ResourceTypeViewModel source)
    //    {
    //        return new RequirementViewModel(source);
    //    }
    //}

    
}