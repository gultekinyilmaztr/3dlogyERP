using _3dlogyERP.Core.Entities;
using _3dlogyERP.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace _3dlogyERP.Application.Dtos.MaterialTypeDtos
{
    public class MaterialTypeCreateDto
    {
        [Required(ErrorMessage = "Malzeme tipi adı zorunludur")]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "Stok kategorisi zorunludur")]
        public StockCategory Category { get; set; }

        [Required(ErrorMessage = "Birim tipi zorunludur")]
        public UnitType Unit { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
