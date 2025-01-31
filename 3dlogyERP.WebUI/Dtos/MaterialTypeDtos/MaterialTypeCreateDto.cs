using _3dlogyERP.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace _3dlogyERP.WebUI.Dtos.MaterialTypeDtos
{
    public class MaterialTypeCreateDto
    {
        [Required(ErrorMessage = "Malzeme tipi adı zorunludur")]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "Stok kategorisi zorunludur")]
        public string StockCategoryCode { get; set; }

        [Required(ErrorMessage = "Birim tipi zorunludur")]
        public UnitType Unit { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
