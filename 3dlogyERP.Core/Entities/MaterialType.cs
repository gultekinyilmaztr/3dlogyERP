using _3dlogyERP.Core.Enums;

namespace _3dlogyERP.Core.Entities
{
    public class MaterialType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        
        public int? StockCategoryId { get; set; }  // Nullable foreign key
        public virtual StockCategory? StockCategory { get; set; }  // Nullable navigation property
        
        public UnitType Unit { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<Material>? Materials { get; set; }
    }
}
