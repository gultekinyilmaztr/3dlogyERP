using System;
using System.Collections.Generic;

namespace _3dlogyERP.Core.Entities
{
    public class Material
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public int MaterialTypeId { get; set; }
        public string Color { get; set; }
        public decimal CostPerKg { get; set; }
        public decimal CurrentStock { get; set; } // in grams
        public decimal MinimumStock { get; set; } // in grams
        public string BatchNumber { get; set; }
        public bool IsActive { get; set; }
        
        // Navigation properties
        public virtual MaterialType MaterialType { get; set; }
        public virtual ICollection<OrderService> Services { get; set; }
    }
}
