namespace _3dlogyERP.Application.Dtos.ExpenseCategoryDtos
{
    // Kategori bazlı özet rapor için kullanılacak DTO
    public class ExpenseCategorySummaryDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public decimal TotalAmount { get; set; }
        public int TransactionCount { get; set; }
        public decimal PercentageOfTotal { get; set; }
    }
}