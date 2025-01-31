namespace _3dlogyERP.WebUI.Dtos.MaterialDtos
{
    public class MaterialListDto
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public int MaterialTypeId { get; set; }
        public string StockCategoryCode { get; set; }
        public string Color { get; set; }
        public decimal UnitCost { get; set; }  // Birim maliyet
        public decimal CurrentStock { get; set; }  // Mevcut stok miktar� (birime g�re)
        public decimal MinimumStock { get; set; }  // Minimum stok seviyesi
        public decimal ReorderPoint { get; set; }  // Yeniden sipari� noktas�
        public string SKU { get; set; }  // Stok Takip Numaras�
        public string BatchNumber { get; set; }  // Parti numaras�
        public bool IsActive { get; set; }
        public decimal WeightPerUnit { get; set; }  // Birim ba��na a��rl�k (gram)
        public string Location { get; set; }  // Depo lokasyonu
        public string Specifications { get; set; }  // Teknik �zellikler
        public string StockStatus => CurrentStock <= MinimumStock ? "Kritik Stok" : "Normal";
    }
}
