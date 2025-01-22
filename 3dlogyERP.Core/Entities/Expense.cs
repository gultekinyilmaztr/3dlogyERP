namespace _3dlogyERP.Core.Entities
{
    public class Expense
    {
        public int Id { get; set; }
        public string ReceiptNumber { get; set; }            // Fiş/Fatura Numarası
        public int ExpenseCategoryId { get; set; }           // Harcama Kategori ID
        public ExpenseCategory Category { get; set; }        // Harcama Kategorisi
        public string Description { get; set; }              // Açıklama
        public decimal Amount { get; set; }                  // Tutar
        public DateTime ExpenseDate { get; set; }            // Harcama Tarihi
        public string Supplier { get; set; }                 // Tedarikçi/Satıcı
        public string PaymentMethod { get; set; }            // Ödeme Yöntemi
        public bool IsApproved { get; set; }                 // Onay Durumu
        public string ApprovedBy { get; set; }               // Onaylayan
        public DateTime? ApprovalDate { get; set; }          // Onay Tarihi
        public string TaxNumber { get; set; }                // Vergi Numarası
        public decimal TaxAmount { get; set; }               // KDV Tutarı
        public string Notes { get; set; }                    // Notlar
        public string AttachmentPath { get; set; }           // Belge/Ek Dosya Yolu
        public int? ProjectId { get; set; }                  // İlişkili Proje ID (varsa)
        public int? OrderId { get; set; }                    // İlişkili Sipariş ID (varsa)
    }
}
