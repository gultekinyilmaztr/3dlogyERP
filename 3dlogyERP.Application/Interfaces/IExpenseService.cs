using _3dlogyERP.Application.Dtos.ExpenseCategoryDtos;
using _3dlogyERP.Application.Dtos.ExpenseDtos;

namespace _3dlogyERP.Application.Interfaces
{
    public interface IExpenseService
    {
        // Existing methods
        Task<ExpenseListDto> GetExpenseByIdAsync(int id);
        Task<IEnumerable<ExpenseListDto>> GetAllExpensesAsync();
        Task<ExpenseListDto> CreateExpenseAsync(ExpenseCreateDto expenseDto);
        Task<ExpenseListDto> UpdateExpenseAsync(int id, ExpenseUpdateDto expenseDto);
        Task<bool> DeleteExpenseAsync(int id);
        Task<IEnumerable<ExpenseListDto>> GetExpensesByCategoryAsync(int categoryId);
        Task<IEnumerable<ExpenseListDto>> GetExpensesByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<decimal> GetTotalExpensesByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<bool> ExistsByIdAsync(int id);

        // New category-related methods
        Task<ExpenseCategoryDetailDto> CreateCategoryAsync(ExpenseCategoryCreateDto category);
        Task<ExpenseCategoryDetailDto> GetCategoryByIdAsync(int id);
        Task<IEnumerable<ExpenseCategoryListDto>> GetAllCategoriesAsync();
        Task<IEnumerable<ExpenseCategoryListDto>> GetMainCategoriesAsync();
        Task<IEnumerable<ExpenseCategoryListDto>> GetSubCategoriesAsync(int parentId);
        Task<bool> UpdateCategoryAsync(int id, ExpenseCategoryUpdateDto category);
        Task<bool> DeleteCategoryAsync(int id);

        // Raporlama metodlarý
        Task<decimal> GetTotalExpensesByCategoryAsync(int categoryId, DateTime? startDate, DateTime? endDate);
        Task<IEnumerable<ExpenseCategorySummaryDto>> GetExpensesSummaryByCategories(DateTime startDate, DateTime endDate);
        Task<IEnumerable<DailyExpenseReportDto>> GetDailyExpensesAsync(DateTime startDate, DateTime endDate);

        // Onaylama iþlemi
        Task<bool> ApproveExpenseAsync(int id, string approvedBy);
    }
}

