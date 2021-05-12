using SiccoApp.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SiccoApp.Models
{
    public class RequirementListViewModel
    {
        public string CustomerID { get; set; }
        public IEnumerable<SelectListItem> Customers { get; set; }

        public string ContractorID { get; set; }
        public IEnumerable<SelectListItem> Contractors { get; set; }

        //public IEnumerable<SelectListItem> RequirementStatus { get; set; }

        public IList<RequirementViewModel> Requirements { get; set; }

        public RequirementListViewModel(IList<Requirement> requirements): this(requirements, string.Empty) { }

        public RequirementListViewModel(IList<Requirement> requirements, string resourceType)
        {
            Requirements = new List<RequirementViewModel>();

            foreach (var item in requirements)
            {
                var requirementViewModel = item.ToDisplayViewModel();

                if (!string.IsNullOrEmpty(resourceType))
                {
                    if (requirementViewModel.ResourceType == resourceType)
                        this.Requirements.Add(item.ToDisplayViewModel());
                }
                else
                {
                    this.Requirements.Add(item.ToDisplayViewModel());

                }
            }
        }
    }
}
