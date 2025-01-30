namespace _3dlogyERP.Application.Dtos.ExpenseDtos
{
    public class ExpenseListDto
    {
        public int Id { get; set; }
        public string ReceiptNumber { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmount => Amount + TaxAmount;
        public DateTime ExpenseDate { get; set; }
        public string Supplier { get; set; }
        public string PaymentMethod { get; set; }
        public bool IsApproved { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public string ApprovalStatus => IsApproved ? "Onaylandı" : "Bekliyor";
    }
}
