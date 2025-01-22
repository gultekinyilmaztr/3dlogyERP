using _3dlogyERP.Core.Entities;
using _3dlogyERP.Core.Interfaces;
using _3dlogyERP.Core.Exceptions;
using System.Linq.Expressions;
using System.Linq;

namespace _3dlogyERP.Application.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly IRepository<Expense> _expenseRepository;
        private readonly IRepository<ExpenseCategory> _categoryRepository;

        public ExpenseService(IRepository<Expense> expenseRepository, IRepository<ExpenseCategory> categoryRepository)
        {
            _expenseRepository = expenseRepository ?? throw new ArgumentNullException(nameof(expenseRepository));
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        }

        // Harcama İşlemleri
        public async Task<Expense> CreateExpenseAsync(Expense expense)
        {
            ArgumentNullException.ThrowIfNull(expense);
            expense.ExpenseDate = expense.ExpenseDate == default ? DateTime.Now : expense.ExpenseDate;
            expense.IsApproved = false;

            await _expenseRepository.AddAsync(expense);
            return expense;
        }

        public async Task<Expense> GetExpenseByIdAsync(int id)
        {
            var expense = await _expenseRepository.GetByIdAsync(id);
            if (expense == null)
                throw new NotFoundException($"Harcama bulunamadı: {id}");
            return expense;
        }

        public async Task<IEnumerable<Expense>> GetAllExpensesAsync()
        {
            return await _expenseRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Expense>> GetExpensesByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _expenseRepository.FindAsync(e => e.ExpenseDate >= startDate && e.ExpenseDate <= endDate);
        }

        public async Task<IEnumerable<Expense>> GetExpensesByCategoryAsync(int categoryId)
        {
            return await _expenseRepository.FindAsync(e => e.ExpenseCategoryId == categoryId);
        }

        public async Task<bool> UpdateExpenseAsync(Expense expense)
        {
            ArgumentNullException.ThrowIfNull(expense);
            _expenseRepository.Update(expense);
            return true;
        }

        public async Task<bool> DeleteExpenseAsync(int id)
        {
            var expense = await _expenseRepository.GetByIdAsync(id);
            if (expense == null)
                return false;

            _expenseRepository.Remove(expense);
            return true;
        }

        public async Task<bool> ApproveExpenseAsync(int id, string approvedBy)
        {
            ArgumentNullException.ThrowIfNull(approvedBy);
            var expense = await _expenseRepository.GetByIdAsync(id);
            if (expense == null)
                return false;

            expense.IsApproved = true;
            expense.ApprovedBy = approvedBy;
            expense.ApprovalDate = DateTime.Now;

            _expenseRepository.Update(expense);
            return true;
        }

        // Kategori İşlemleri
        public async Task<ExpenseCategory> CreateCategoryAsync(ExpenseCategory category)
        {
            ArgumentNullException.ThrowIfNull(category);
            await _categoryRepository.AddAsync(category);
            return category;
        }

        public async Task<ExpenseCategory> GetCategoryByIdAsync(int id)
        {
            return await _categoryRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<ExpenseCategory>> GetAllCategoriesAsync()
        {
            return await _categoryRepository.GetAllAsync();
        }

        public async Task<IEnumerable<ExpenseCategory>> GetMainCategoriesAsync()
        {
            return await _categoryRepository.FindAsync(c => c.ParentCategoryId == null);
        }

        public async Task<IEnumerable<ExpenseCategory>> GetSubCategoriesAsync(int parentId)
        {
            return await _categoryRepository.FindAsync(c => c.ParentCategoryId == parentId);
        }

        public async Task<bool> UpdateCategoryAsync(ExpenseCategory category)
        {
            ArgumentNullException.ThrowIfNull(category);
            _categoryRepository.Update(category);
            return true;
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
                return false;

            var hasSubCategories = await _categoryRepository.AnyAsync(c => c.ParentCategoryId == id);
            var hasExpenses = await _expenseRepository.AnyAsync(e => e.ExpenseCategoryId == id);

            if (hasSubCategories || hasExpenses)
                return false;

            _categoryRepository.Remove(category);
            return true;
        }

        // Raporlama İşlemleri
        public async Task<decimal> GetTotalExpensesByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var expenses = await GetExpensesByDateRangeAsync(startDate, endDate);
            return expenses.Sum(e => e.Amount);
        }

        public async Task<decimal> GetTotalExpensesByCategoryAsync(int categoryId, DateTime? startDate = null, DateTime? endDate = null)
        {
            var expenses = await GetExpensesByCategoryAsync(categoryId);
            var filteredExpenses = expenses.AsQueryable();

            if (startDate.HasValue)
                filteredExpenses = filteredExpenses.Where(e => e.ExpenseDate >= startDate.Value);
            if (endDate.HasValue)
                filteredExpenses = filteredExpenses.Where(e => e.ExpenseDate <= endDate.Value);

            return filteredExpenses.Sum(e => e.Amount);
        }

        public async Task<IDictionary<string, decimal>> GetExpensesSummaryByCategories(DateTime startDate, DateTime endDate)
        {
            var expenses = await GetExpensesByDateRangeAsync(startDate, endDate);
            return expenses.GroupBy(e => e.Category.Name)
                         .ToDictionary(g => g.Key, g => g.Sum(e => e.Amount));
        }

        public async Task<IDictionary<DateTime, decimal>> GetDailyExpensesAsync(DateTime startDate, DateTime endDate)
        {
            var expenses = await GetExpensesByDateRangeAsync(startDate, endDate);
            return expenses.GroupBy(e => e.ExpenseDate.Date)
                         .ToDictionary(g => g.Key, g => g.Sum(e => e.Amount));
        }
    }
}
