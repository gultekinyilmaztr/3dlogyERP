using System.ComponentModel.DataAnnotations;

namespace _3dlogyERP.Application.Dtos.MaterialDtos
{
    public class MaterialUpdateDto
    {
        public int Id { get; set; }  
        public string Name { get; set; }
        public string Brand { get; set; }
        public int MaterialTypeId { get; set; } 
        public int StockCategoryId { get; set; }
        public string Color { get; set; }
        public decimal UnitCost { get; set; }
        public decimal CurrentStock { get; set; }
        public decimal MinimumStock { get; set; }
        public decimal ReorderPoint { get; set; }
        public string SKU { get; set; }
        public string BatchNumber { get; set; }
        public bool IsActive { get; set; }
        public decimal WeightPerUnit { get; set; }
        public string Location { get; set; }
        public string Specifications { get; set; }
    }
}
