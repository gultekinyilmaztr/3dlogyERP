using _3dlogyERP.Core.Entities;
using _3dlogyERP.Core.Interfaces;
using _3dlogyERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace _3dlogyERP.Application.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly ApplicationDbContext _context;

        public ExpenseService(ApplicationDbContext context)
        {
            ArgumentNullException.ThrowIfNull(context);
            _context = context;
        }

        // Harcama İşlemleri
        public async Task<Expense> CreateExpenseAsync(Expense expense)
        {
            ArgumentNullException.ThrowIfNull(expense);
            expense.ExpenseDate = expense.ExpenseDate == default ? DateTime.Now : expense.ExpenseDate;
            expense.IsApproved = false;

            await _context.Expenses.AddAsync(expense);
            await _context.SaveChangesAsync();
            return expense;
        }

        public async Task<Expense> GetExpenseByIdAsync(int id)
        {
            return await _context.Expenses
                .Include(e => e.Category)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<Expense>> GetAllExpensesAsync()
        {
            return await _context.Expenses
                .Include(e => e.Category)
                .OrderByDescending(e => e.ExpenseDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Expense>> GetExpensesByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Expenses
                .Include(e => e.Category)
                .Where(e => e.ExpenseDate >= startDate && e.ExpenseDate <= endDate)
                .OrderByDescending(e => e.ExpenseDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Expense>> GetExpensesByCategoryAsync(int categoryId)
        {
            return await _context.Expenses
                .Include(e => e.Category)
                .Where(e => e.ExpenseCategoryId == categoryId)
                .OrderByDescending(e => e.ExpenseDate)
                .ToListAsync();
        }

        public async Task<bool> UpdateExpenseAsync(Expense expense)
        {
            ArgumentNullException.ThrowIfNull(expense);
            var existingExpense = await _context.Expenses.FindAsync(expense.Id);
            if (existingExpense == null)
                return false;

            _context.Entry(existingExpense).CurrentValues.SetValues(expense);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteExpenseAsync(int id)
        {
            var expense = await _context.Expenses.FindAsync(id);
            if (expense == null)
                return false;

            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ApproveExpenseAsync(int id, string approvedBy)
        {
            ArgumentNullException.ThrowIfNull(approvedBy);
            var expense = await _context.Expenses.FindAsync(id);
            if (expense == null)
                return false;

            expense.IsApproved = true;
            expense.ApprovedBy = approvedBy;
            expense.ApprovalDate = DateTime.Now;

            await _context.SaveChangesAsync();
            return true;
        }

        // Kategori İşlemleri
        public async Task<ExpenseCategory> CreateCategoryAsync(ExpenseCategory category)
        {
            ArgumentNullException.ThrowIfNull(category);
            await _context.ExpenseCategories.AddAsync(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<ExpenseCategory> GetCategoryByIdAsync(int id)
        {
            return await _context.ExpenseCategories
                .Include(c => c.SubCategories)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<ExpenseCategory>> GetAllCategoriesAsync()
        {
            return await _context.ExpenseCategories
                .Include(c => c.SubCategories)
                .ToListAsync();
        }

        public async Task<IEnumerable<ExpenseCategory>> GetMainCategoriesAsync()
        {
            return await _context.ExpenseCategories
                .Include(c => c.SubCategories)
                .Where(c => c.ParentCategoryId == null)
                .ToListAsync();
        }

        public async Task<IEnumerable<ExpenseCategory>> GetSubCategoriesAsync(int parentId)
        {
            return await _context.ExpenseCategories
                .Where(c => c.ParentCategoryId == parentId)
                .ToListAsync();
        }

        public async Task<bool> UpdateCategoryAsync(ExpenseCategory category)
        {
            ArgumentNullException.ThrowIfNull(category);
            var existingCategory = await _context.ExpenseCategories.FindAsync(category.Id);
            if (existingCategory == null)
                return false;

            _context.Entry(existingCategory).CurrentValues.SetValues(category);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _context.ExpenseCategories
                .Include(c => c.SubCategories)
                .Include(c => c.Expenses)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
                return false;

            // Alt kategorileri veya harcamaları varsa silme
            if (category.SubCategories.Any() || category.Expenses.Any())
                return false;

            _context.ExpenseCategories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }

        // Raporlama İşlemleri
        public async Task<decimal> GetTotalExpensesByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Expenses
                .Where(e => e.ExpenseDate >= startDate && e.ExpenseDate <= endDate)
                .SumAsync(e => e.Amount);
        }

        public async Task<decimal> GetTotalExpensesByCategoryAsync(int categoryId, DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = _context.Expenses.Where(e => e.ExpenseCategoryId == categoryId);

            if (startDate.HasValue)
                query = query.Where(e => e.ExpenseDate >= startDate.Value);
            if (endDate.HasValue)
                query = query.Where(e => e.ExpenseDate <= endDate.Value);

            return await query.SumAsync(e => e.Amount);
        }

        public async Task<IDictionary<string, decimal>> GetExpensesSummaryByCategories(DateTime startDate, DateTime endDate)
        {
            var expenses = await _context.Expenses
                .Include(e => e.Category)
                .Where(e => e.ExpenseDate >= startDate && e.ExpenseDate <= endDate)
                .GroupBy(e => e.Category.Name)
                .Select(g => new { CategoryName = g.Key, TotalAmount = g.Sum(e => e.Amount) })
                .ToDictionaryAsync(x => x.CategoryName, x => x.TotalAmount);

            return expenses;
        }

        public async Task<IDictionary<DateTime, decimal>> GetDailyExpensesAsync(DateTime startDate, DateTime endDate)
        {
            var expenses = await _context.Expenses
                .Where(e => e.ExpenseDate >= startDate && e.ExpenseDate <= endDate)
                .GroupBy(e => e.ExpenseDate.Date)
                .Select(g => new { Date = g.Key, TotalAmount = g.Sum(e => e.Amount) })
                .ToDictionaryAsync(x => x.Date, x => x.TotalAmount);

            return expenses;
        }
    }
}
