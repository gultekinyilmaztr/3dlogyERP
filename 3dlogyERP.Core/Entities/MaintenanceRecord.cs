namespace _3dlogyERP.Core.Entities
{
    public class MaintenanceRecord
    {
        public int Id { get; set; }
        public int EquipmentId { get; set; }
        public DateTime MaintenanceDate { get; set; }
        public string Description { get; set; }
        public string Technician { get; set; }
        public decimal Cost { get; set; }
        public string Notes { get; set; }
        public bool IsScheduled { get; set; }

        // Navigation property
        public virtual Equipment Equipment { get; set; }
    }
}
