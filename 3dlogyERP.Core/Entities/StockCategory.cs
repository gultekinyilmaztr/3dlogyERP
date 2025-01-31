namespace _3dlogyERP.Core.Entities
{
    public class StockCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<Material> Materials { get; set; }
        public virtual ICollection<MaterialType> MaterialTypes { get; set; }
    }
}
