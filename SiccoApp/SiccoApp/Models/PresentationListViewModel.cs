using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SiccoApp.Persistence;

namespace SiccoApp.Models
{
    public class PresentationListViewModel
    {
        public IList<PresentationViewModel> Presentations { get; set; }

        public PresentationListViewModel(IList<Presentation> presentations)
        {
            Presentations = new List<PresentationViewModel>();
            foreach (var item in presentations)
            {
                this.Presentations.Add(item.ToDisplayViewModel());
            }
        }
    }
}
