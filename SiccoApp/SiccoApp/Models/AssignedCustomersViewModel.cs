using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using SiccoApp.Persistence;

namespace SiccoApp.Models
{
    public class AssignedCustomersViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public IEnumerable<CustomerAuditor> CustomerAuditors { get; set; }

        public AssignedCustomersViewModel(ApplicationUser user, List<CustomerAuditor> customerAuditors)
        {
            this.UserId = user.Id;
            this.UserName = user.UserName;
            this.CustomerAuditors = customerAuditors;
        }
    }

    public class EditAssignCustomerViewModel
    {
        public int CustomerAuditorID { get; set; }

        [Display(Name = "EtyCustomer", ResourceType = typeof(Resources.Resources))]
        public int CustomerID { get; set; }
        public string UserID { get; set; }

        [Display(Name = "EtyUser", ResourceType = typeof(Resources.Resources))]
        public string UserName { get; set; }


        [Display(Name = "EtyCustomer", ResourceType = typeof(Resources.Resources))]
        public string CustomerDesc { get; set; }

        public EditAssignCustomerViewModel() { }

        public EditAssignCustomerViewModel(CustomerAuditor customerAuditor)
        {
            this.CustomerAuditorID = (int)customerAuditor.CustomerAuditorID;
            this.CustomerID = customerAuditor.CustomerID;
            this.UserID = customerAuditor.UserId;
            this.UserName = customerAuditor.User.UserName;
            this.CustomerDesc = customerAuditor.Customer.CompanyName + " (" + customerAuditor.Customer.TaxIdNumber + ")";
        }

        public CustomerAuditor GetCustomer()
        {
            var customer = new CustomerAuditor()
            {
                CustomerAuditorID = this.CustomerAuditorID,
                CustomerID = this.CustomerID,
                UserId = this.UserID 
           };
            return customer;
        }

    }

    //public class EditAssignDocumentTemplateViewModel
    //{
    //    public int DocumentationBusinessTypeTemplateID { get; set; }

    //    [Display(Name = "Documentation", ResourceType = typeof(Resources.Resources))]
    //    public int DocumentationTemplateID { get; set; }

    //    public int BusinessTypeTemplateID { get; set; }

    //    [Display(Name = "Importance", ResourceType = typeof(Resources.Resources))]
    //    [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "Importance_Required")]
    //    [Range(1, 10, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "Importance_Range")]
    //    public byte Importance { get; set; }

    //    public int DocumentationImportanceID { get; set; }


    //    [Display(Name = "Documentation", ResourceType = typeof(Resources.Resources))]
    //    public string DocumentationTemplateDesc { get; set; }

    //    public EditAssignDocumentTemplateViewModel() { }

    //    public EditAssignDocumentTemplateViewModel(DocumentationBusinessTypeTemplate docuBusinessType)
    //    {
    //        this.DocumentationBusinessTypeTemplateID = docuBusinessType.DocumentationBusinessTypeTemplateID;
    //        this.DocumentationTemplateID = docuBusinessType.DocumentationTemplateID;
    //        this.BusinessTypeTemplateID = docuBusinessType.BusinessTypeTemplateID;
    //        this.Importance = docuBusinessType.Importance;
    //        this.DocumentationImportanceID = docuBusinessType.DocumentationImportanceID;
    //        this.DocumentationTemplateDesc = docuBusinessType.DocumentationTemplate.DocumentationTemplateCode + " - (" + docuBusinessType.DocumentationTemplate.Description + ")";
    //    }

    //    public virtual DocumentationBusinessTypeTemplate GetDocument()
    //    {
    //        var document = new DocumentationBusinessTypeTemplate()
    //        {
    //            DocumentationBusinessTypeTemplateID = this.DocumentationBusinessTypeTemplateID,
    //            DocumentationTemplateID = this.DocumentationTemplateID,
    //            BusinessTypeTemplateID = this.BusinessTypeTemplateID,
    //            Importance = this.Importance,
    //            DocumentationImportanceID = this.DocumentationImportanceID
    //        };
    //        return document;
    //    }
    //}
}