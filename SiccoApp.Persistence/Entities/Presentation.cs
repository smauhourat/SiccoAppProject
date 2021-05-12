using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace SiccoApp.Persistence
{


    //PENDING       (Cuando se sube el documento y queda disponible) en este estado aun puede ser eliminado por el Contratista
    //TOPROCESS     (Marcado listo para ser tomado por un Auditor) Este estado en ppio puede ser manejado de forma automatica
    //PROCESSING    (Cuando es tomado por un Auditor para su analisis) dispara el PROCESSING del Requerimiento
    //APPROVED      (Cuando el Auditor aprueba el Requerimiento en base al analisis del documento presentado) ESTADO FINAL, dispara el APPROVED del Requerimiento
    //REJECTED      (Cuando el Auditor NO aprueba el Requerimiento) ESTADO FINAL, dipara el PENDING del Requerimiento.

    public enum PresentationStatus
    {
        [Description("Pendiente")]
        Pending = 1,

        [Description("A Procesar")]
        ToProccess = 2,

        [Description("En Proceso")]
        Processing = 3,

        [Description("Aprobada")]
        Approved = 4,

        [Description("Rechazada")]
        Rejected = 5
    }

    public class Presentation
    {
        public int PresentationID { get; set; }
        public int RequirementID { get; set; }
        public virtual Requirement Requirement { get; set; }
        public PresentationStatus PresentationStatus { get; set; }
        public System.DateTime PresentationDate { get; set; }
        public string DocumentFiles { get; set; }

        //[ForeignKey("TakenFor")]
        public string TakenForID { get; set; }
        public ApplicationUser TakenFor { get; set; }

        public Nullable<System.DateTime> TakenDate { get; set; }

        //[ForeignKey("ApprovedFor")]
        public string ApprovedForID { get; set; }
        public ApplicationUser ApprovedFor { get; set; }

        public Nullable<System.DateTime> ApprovedDate { get; set; }

        public string RejectedForID { get; set; }
        public ApplicationUser RejectedFor { get; set; }

        //public Nullable<int> RejectedForID { get; set; }

        public Nullable<System.DateTime> RejectedDate { get; set; }
        public string Observations { get; set; }



        public ICollection<PresentationAction> PresentationActions { get; set; }
    }
}
