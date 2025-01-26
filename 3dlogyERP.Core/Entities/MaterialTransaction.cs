namespace _3dlogyERP.Core.Entities
{
    public enum TransactionType
    {
        StockIn,    // Stok Girişi
        StockOut,   // Stok Çıkışı
        Return,     // İade
        Adjustment  // Stok Düzeltme
    }

    public class MaterialTransaction
    {
        public int Id { get; set; }
        public int MaterialId { get; set; }
        public TransactionType Type { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string ReferenceNumber { get; set; }  // Fatura/Sipariş No
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation properties
        public virtual Material Material { get; set; }
    }
}
