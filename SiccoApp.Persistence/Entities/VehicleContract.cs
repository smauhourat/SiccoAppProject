namespace SiccoApp.Persistence
{
    public class VehicleContract
    {
        public int VehicleContractID { get; set; }
        public int VehicleID { get; set; }
        public int ContractID { get; set; }

        public virtual Vehicle Vehicle { get; set; }
        public virtual Contract Contract { get; set; }
    }
}