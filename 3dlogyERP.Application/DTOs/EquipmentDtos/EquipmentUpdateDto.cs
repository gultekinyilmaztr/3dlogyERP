using System.ComponentModel.DataAnnotations;

namespace _3dlogyERP.Application.Dtos.EquipmentDtos
{
    public class EquipmentUpdateDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ekipman adı zorunludur")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Model bilgisi zorunludur")]
        public string Model { get; set; }

        [Required(ErrorMessage = "Seri numarası zorunludur")]
        public string SerialNumber { get; set; }

        [Required(ErrorMessage = "Ekipman tipi zorunludur")]
        public int EquipmentTypeId { get; set; }

        [Required(ErrorMessage = "Satın alma tarihi zorunludur")]
        public DateTime PurchaseDate { get; set; }

        [Required(ErrorMessage = "Satın alma fiyatı zorunludur")]
        public decimal PurchasePrice { get; set; }

        [Required(ErrorMessage = "Saatlik ücret zorunludur")]
        public decimal HourlyRate { get; set; }

        [Required(ErrorMessage = "Saatlik bakım maliyeti zorunludur")]
        public decimal MaintenanceCostPerHour { get; set; }

        [Required(ErrorMessage = "Saatlik elektrik tüketimi zorunludur")]
        public decimal ElectricityConsumptionPerHour { get; set; }

        public bool IsActive { get; set; }
        public bool IsAvailable { get; set; }

        [Required(ErrorMessage = "Son bakım tarihi zorunludur")]
        public DateTime LastMaintenanceDate { get; set; }

        [Required(ErrorMessage = "Sonraki bakım tarihi zorunludur")]
        public DateTime NextMaintenanceDate { get; set; }
    }
}
