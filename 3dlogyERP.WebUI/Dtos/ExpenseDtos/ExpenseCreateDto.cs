using System.ComponentModel.DataAnnotations;

namespace _3dlogyERP.WebUI.Dtos.ExpenseDtos
{
    public class ExpenseCreateDto
    {
        [Required(ErrorMessage = "Fiş/Fatura numarası zorunludur")]
        public string ReceiptNumber { get; set; }

        [Required(ErrorMessage = "Harcama kategorisi zorunludur")]
        public int ExpenseCategoryId { get; set; }

        [Required(ErrorMessage = "Açıklama zorunludur")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Tutar zorunludur")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Harcama tarihi zorunludur")]
        public DateTime ExpenseDate { get; set; }

        [Required(ErrorMessage = "Tedarikçi/Satıcı zorunludur")]
        public string Supplier { get; set; }

        [Required(ErrorMessage = "Ödeme yöntemi zorunludur")]
        public string PaymentMethod { get; set; }

        public string TaxNumber { get; set; }
        public decimal TaxAmount { get; set; }
        public string Notes { get; set; }
        public string AttachmentPath { get; set; }
        public int? ProjectId { get; set; }
        public int? OrderId { get; set; }
    }
}
