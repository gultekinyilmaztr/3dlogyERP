using System;
using System.Collections.Generic;

namespace _3dlogyERP.Core.Entities
{
    public class Equipment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Model { get; set; }
        public string SerialNumber { get; set; }
        public int EquipmentTypeId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal HourlyRate { get; set; }
        public decimal MaintenanceCostPerHour { get; set; }
        public decimal ElectricityConsumptionPerHour { get; set; }
        public bool IsActive { get; set; }
        public DateTime LastMaintenanceDate { get; set; }
        public DateTime NextMaintenanceDate { get; set; }
        
        // Navigation properties
        public virtual EquipmentType EquipmentType { get; set; }
        public virtual ICollection<OrderService> Services { get; set; }
        public virtual ICollection<MaintenanceRecord> MaintenanceRecords { get; set; }
    }
}
