using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiccoApp.Persistence
{
    public enum PresentationActionType
    {
        Taken = 1,
        Approve = 2,
        Reject = 3,
        Drop = 4,
    }
    public class PresentationAction
    {
        public int PresentationActionID { get; set; }
        public int PresentationID { get; set; }
        public DateTime PresentationDate { get; set; }
        public string ActionForID { get; set; }
        public PresentationActionType PresentationActionType { get; set; }

        public Presentation Presentation { get; set; }        
    }
}
