namespace _3dlogyERP.WebUI.Dtos.EquipmentDtos
{
    public class EquipmentListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Model { get; set; }
        public string SerialNumber { get; set; }
        public string EquipmentTypeName { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal HourlyRate { get; set; }
        public bool IsActive { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime LastMaintenanceDate { get; set; }
        public DateTime NextMaintenanceDate { get; set; }
        public int DaysUntilMaintenance => (NextMaintenanceDate - DateTime.Now).Days;
        public string MaintenanceStatus => DaysUntilMaintenance <= 7 ? "Bakım Yakın" : "Normal";
    }
}
