namespace _3dlogyERP.Application.Dtos.ExpenseCategoryDtos
{
    // Günlük gider raporu için kullanılacak DTO
    public class DailyExpenseReportDto
    {
        public DateTime Date { get; set; }
        public decimal TotalAmount { get; set; }
        public Dictionary<string, decimal> CategoryBreakdown { get; set; }
    }
}