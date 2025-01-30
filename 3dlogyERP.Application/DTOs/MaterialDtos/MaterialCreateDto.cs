using _3dlogyERP.Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace _3dlogyERP.Application.Dtos.MaterialDtos
{
    public class MaterialCreateDto
    {
        [Required(ErrorMessage = "Malzeme adı zorunludur")]
        public string Name { get; set; }

        public string Brand { get; set; }

        [Required(ErrorMessage = "Malzeme tipi zorunludur")]
        public int MaterialTypeId { get; set; }

        [Required(ErrorMessage = "Stok Tipi zorunludur")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public StockCategory StockCategory { get; set; }

        public string Color { get; set; }

        [Required(ErrorMessage = "Birim maliyet zorunludur")]
        public decimal UnitCost { get; set; }

        [Required(ErrorMessage = "Mevcut stok miktarı zorunludur")]
        public decimal CurrentStock { get; set; }

        [Required(ErrorMessage = "Minimum stok seviyesi zorunludur")]
        public decimal MinimumStock { get; set; }

        public decimal ReorderPoint { get; set; }

        [Required(ErrorMessage = "SKU zorunludur")]
        public string SKU { get; set; }

        public string BatchNumber { get; set; }
        public bool IsActive { get; set; } = true;
        public decimal WeightPerUnit { get; set; }
        public string Location { get; set; }
        public string Specifications { get; set; }
    }
}
