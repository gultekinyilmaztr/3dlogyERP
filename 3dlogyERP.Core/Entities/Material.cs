namespace _3dlogyERP.Core.Entities
{
    public class Material
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public int MaterialTypeId { get; set; }
        public string Color { get; set; }
        public decimal UnitCost { get; set; }  // Birim maliyet
        public decimal CurrentStock { get; set; }  // Mevcut stok miktarı (birime göre)
        public decimal MinimumStock { get; set; }  // Minimum stok seviyesi
        public decimal ReorderPoint { get; set; }  // Yeniden sipariş noktası
        public string SKU { get; set; }  // Stok Takip Numarası
        public string BatchNumber { get; set; }  // Parti numarası
        public bool IsActive { get; set; }
        public decimal WeightPerUnit { get; set; }  // Birim başına ağırlık (gram)
        public string Location { get; set; }  // Depo lokasyonu
        public string Specifications { get; set; }  // Teknik özellikler

        // Navigation properties
        public virtual MaterialType MaterialType { get; set; }
        public virtual ICollection<OrderService> Services { get; set; }
        public virtual ICollection<MaterialTransaction> Transactions { get; set; }
    }
}
