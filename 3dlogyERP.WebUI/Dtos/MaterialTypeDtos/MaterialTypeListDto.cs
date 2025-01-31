using _3dlogyERP.Core.Enums;

namespace _3dlogyERP.WebUI.Dtos.MaterialTypeDtos
{
    public class MaterialTypeListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string StockCategoryCode { get; set; }
        public UnitType Unit { get; set; }
        public string UnitName => Unit.ToString();
        public bool IsActive { get; set; }
    }
}
