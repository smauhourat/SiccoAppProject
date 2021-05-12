using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel;

namespace SiccoApp.Persistence
{

    //CREATED       (Cuando se crea un requerimiento pero aun no esta disponible para el Sistema)
    //PENDING       (Cuando pasa a estar disponible para el Sistema, y no tiene presentaciones para procesar)
    //TOPROCESS     (Cuando se le adjunto una Presentacion pero aun no ha sido "tomada" por un Auditor)
    //PROCESSING    (Cuando la Presentacion adjunta es tomada por un Auditor para su analisis)
    //APPROVED      (Cuando tiene una Presentacion Aprobada) ESTADO FINAL
    //REJECTED      (Cuando no tiene ninguna Presentacion Aprobada y ademas se vencio la fecha de presentaciones) ESTADO FINAL

    public enum RequirementStatus
    {
        [Description("Creado")]
        Created = 1,

        [Description("Pendiente")]
        Pending = 2,

        [Description("A Procesar")]
        ToProcess = 3,

        [Description("En Proceso")]
        Processing = 4,

        [Description("Aprobado")]
        Approved = 5,

        [Description("Rechazado")]
        Rejected = 6
    }

    [MetadataType(typeof(RequirementMetaData))]
    public class Requirement
    {
        public int RequirementID { get; set; }
        public int DocumentationBusinessTypeID { get; set; }        
        public int ContractID { get; set; }
        public Nullable<int> EmployeeContractID { get; set; }
        public Nullable<int> VehicleContractID { get; set; }


        public int PeriodID { get; set; }
        public RequirementStatus RequirementStatus { get; set; }
        public DateTime DueDate { get; set; }

        public virtual ICollection<Presentation> Presentations { get; set; }
        public virtual DocumentationBusinessType DocumentationBusinessType { get; set; }
        public virtual EmployeeContract EmployeeContract { get; set; }
        public virtual VehicleContract VehicleContract { get; set; }
        public virtual Contract Contract { get; set; }
    }

}
