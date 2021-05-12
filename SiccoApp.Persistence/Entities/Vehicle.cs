using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace SiccoApp.Persistence
{
    [MetadataType(typeof(VehicleMetaData))]
    public class Vehicle
    {
        public int VehicleID { get; set; }
        public int ContractorID { get; set; }
        public string IdentificationNumber { get; set; }
        public string Description { get; set; }
        public Nullable<byte> Active { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public string CreationUser { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string ModifiedUser { get; set; }
        public bool Disabled { get; set; }
        public Nullable<DateTime> DisabledDate { get; set; }

        public Contractor Contractor { get; set; }
        //public virtual ICollection<VehicleContract> VehicleContract { get; set; }
    }
}
