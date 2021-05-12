using SiccoApp.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiccoApp.Models
{
    public class DocumentationResumeViewModel
    {

        //public string DocumentationFullDescription { get; set; }

        public ICollection<DocumentationResume> ContractorDocumentation { get; set; }
        public ICollection<DocumentationResume> EmployeDocumentation { get; set; }
        public ICollection<DocumentationResume> VehicleDocumentation { get; set; }

        public DocumentationResumeViewModel(List<DocumentationResume> documentationResume)
        {
            this.ContractorDocumentation = documentationResume.Where(x => x.EntityTypeDescription == ResourceTypeViewModel.Contractor).ToList();
            this.EmployeDocumentation = documentationResume.Where(x => x.EntityTypeDescription == ResourceTypeViewModel.Employee).ToList();
            this.VehicleDocumentation = documentationResume.Where(x => x.EntityTypeDescription == ResourceTypeViewModel.Vehicle).ToList();
        }

    }
}