using _3dlogyERP.Core.Entities;

namespace _3dlogyERP.Core.Interfaces
{
    public interface IExpenseService
    {
        // Harcama İşlemleri
        Task<Expense> CreateExpenseAsync(Expense expense);
        Task<Expense> GetExpenseByIdAsync(int id);
        Task<IEnumerable<Expense>> GetAllExpensesAsync();
        Task<IEnumerable<Expense>> GetExpensesByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Expense>> GetExpensesByCategoryAsync(int categoryId);
        Task<bool> UpdateExpenseAsync(Expense expense);
        Task<bool> DeleteExpenseAsync(int id);
        Task<bool> ApproveExpenseAsync(int id, string approvedBy);

        // Kategori İşlemleri
        Task<ExpenseCategory> CreateCategoryAsync(ExpenseCategory category);
        Task<ExpenseCategory> GetCategoryByIdAsync(int id);
        Task<IEnumerable<ExpenseCategory>> GetAllCategoriesAsync();
        Task<IEnumerable<ExpenseCategory>> GetMainCategoriesAsync();
        Task<IEnumerable<ExpenseCategory>> GetSubCategoriesAsync(int parentId);
        Task<bool> UpdateCategoryAsync(ExpenseCategory category);
        Task<bool> DeleteCategoryAsync(int id);

        // Raporlama İşlemleri
        Task<decimal> GetTotalExpensesByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<decimal> GetTotalExpensesByCategoryAsync(int categoryId, DateTime? startDate = null, DateTime? endDate = null);
        Task<IDictionary<string, decimal>> GetExpensesSummaryByCategories(DateTime startDate, DateTime endDate);
        Task<IDictionary<DateTime, decimal>> GetDailyExpensesAsync(DateTime startDate, DateTime endDate);
    }
}
