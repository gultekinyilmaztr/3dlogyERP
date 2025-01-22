using _3dlogyERP.Core.Enums;

namespace _3dlogyERP.Core.Entities
{
    public class OrderService
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ServiceTypeId { get; set; }
        public string Description { get; set; }
        public decimal TotalCost { get; set; }
        public decimal MaterialCost { get; set; }
        public decimal LaborCost { get; set; }
        public decimal EquipmentCost { get; set; }
        public decimal Price { get; set; }
        public decimal MaterialQuantity { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public TimeSpan EstimatedDuration { get; set; }
        public ServiceStatus Status { get; set; }
        public int? EquipmentId { get; set; }
        public int? MaterialId { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation properties
        public virtual Order Order { get; set; }
        public virtual ServiceType ServiceType { get; set; }
        public virtual Equipment Equipment { get; set; }
        public virtual Material Material { get; set; }
    }
}
