using _3dlogyERP.Core.Enums;

namespace _3dlogyERP.WebUI.Dtos.MaterialTypeDtos
{
    public class MaterialTypeListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public StockCategory Category { get; set; }
        public string CategoryName => Category.ToString();
        public UnitType Unit { get; set; }
        public string UnitName => Unit.ToString();
        public bool IsActive { get; set; }
    }
}
