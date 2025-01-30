using _3dlogyERP.Application.Dtos.ExpenseCategoryDtos;
using _3dlogyERP.Application.Dtos.ExpenseDtos;
using _3dlogyERP.Application.Interfaces;
using _3dlogyERP.Core.Entities;
using _3dlogyERP.Core.Exceptions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

public class ExpenseService : IExpenseService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ExpenseService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        ArgumentNullException.ThrowIfNull(unitOfWork);
        ArgumentNullException.ThrowIfNull(mapper);

        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    #region Expense Operations
    public async Task<ExpenseListDto> GetExpenseByIdAsync(int id)
    {
        var expense = await _unitOfWork.Expenses
            .Query()
            .Include(e => e.Category)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (expense == null)
            throw new NotFoundException($"Expense with ID {id} not found");

        return _mapper.Map<ExpenseListDto>(expense);
    }

    public async Task<IEnumerable<ExpenseListDto>> GetAllExpensesAsync()
    {
        var expenses = await _unitOfWork.Expenses
            .Query()
            .Include(e => e.Category)
            .ToListAsync();

        return _mapper.Map<IEnumerable<ExpenseListDto>>(expenses);
    }

    public async Task<ExpenseListDto> CreateExpenseAsync(ExpenseCreateDto expenseDto)
    {
        ArgumentNullException.ThrowIfNull(expenseDto);

        var expense = _mapper.Map<Expense>(expenseDto);
        await _unitOfWork.Expenses.AddAsync(expense);
        await _unitOfWork.SaveChangesAsync();

        return await GetExpenseByIdAsync(expense.Id);
    }

    public async Task<ExpenseListDto> UpdateExpenseAsync(int id, ExpenseUpdateDto expenseDto)
    {
        ArgumentNullException.ThrowIfNull(expenseDto);

        var existingExpense = await _unitOfWork.Expenses.GetByIdAsync(id);
        if (existingExpense == null)
            throw new NotFoundException($"Expense with ID {id} not found");

        _mapper.Map(expenseDto, existingExpense);
        _unitOfWork.Expenses.Update(existingExpense);
        await _unitOfWork.SaveChangesAsync();

        return await GetExpenseByIdAsync(id);
    }

    public async Task<bool> DeleteExpenseAsync(int id)
    {
        var expense = await _unitOfWork.Expenses.GetByIdAsync(id);
        if (expense == null)
            return false;

        _unitOfWork.Expenses.Remove(expense);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<ExpenseListDto>> GetExpensesByCategoryAsync(int categoryId)
    {
        var expenses = await _unitOfWork.Expenses
            .Query()
            .Include(e => e.Category)
            .Where(e => e.ExpenseCategoryId == categoryId)
            .ToListAsync();

        return _mapper.Map<IEnumerable<ExpenseListDto>>(expenses);
    }

    public async Task<IEnumerable<ExpenseListDto>> GetExpensesByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        var expenses = await _unitOfWork.Expenses
            .Query()
            .Include(e => e.Category)
            .Where(e => e.ExpenseDate >= startDate && e.ExpenseDate <= endDate)
            .ToListAsync();

        return _mapper.Map<IEnumerable<ExpenseListDto>>(expenses);
    }

    public async Task<decimal> GetTotalExpensesByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _unitOfWork.Expenses
            .Query()
            .Where(e => e.ExpenseDate >= startDate && e.ExpenseDate <= endDate)
            .SumAsync(e => e.Amount);
    }

    public async Task<bool> ExistsByIdAsync(int id)
    {
        return await _unitOfWork.Expenses
            .Query()
            .AnyAsync(e => e.Id == id);
    }
    #endregion

    #region Category Operations
    public async Task<ExpenseCategoryDetailDto> CreateCategoryAsync(ExpenseCategoryCreateDto categoryDto)
    {
        ArgumentNullException.ThrowIfNull(categoryDto);

        var category = _mapper.Map<ExpenseCategory>(categoryDto);
        await _unitOfWork.ExpenseCategories.AddAsync(category);
        await _unitOfWork.SaveChangesAsync();

        return await GetCategoryByIdAsync(category.Id);
    }

    public async Task<ExpenseCategoryDetailDto> GetCategoryByIdAsync(int id)
    {
        var category = await _unitOfWork.ExpenseCategories
            .Query()
            .Include(c => c.ParentCategory)
            .Include(c => c.SubCategories)
            .Include(c => c.Expenses)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (category == null)
            throw new NotFoundException($"Category with ID {id} not found");

        var categoryDto = _mapper.Map<ExpenseCategoryDetailDto>(category);
        categoryDto.TotalExpenseAmount = category.Expenses?.Sum(e => e.Amount) ?? 0;
        return categoryDto;
    }

    public async Task<IEnumerable<ExpenseCategoryListDto>> GetAllCategoriesAsync()
    {
        var categories = await _unitOfWork.ExpenseCategories
            .Query()
            .Include(c => c.ParentCategory)
            .Include(c => c.SubCategories)
            .Include(c => c.Expenses)
            .ToListAsync();

        return _mapper.Map<IEnumerable<ExpenseCategoryListDto>>(categories);
    }

    public async Task<IEnumerable<ExpenseCategoryListDto>> GetMainCategoriesAsync()
    {
        var mainCategories = await _unitOfWork.ExpenseCategories
            .Query()
            .Include(c => c.SubCategories)
            .Include(c => c.Expenses)
            .Where(c => c.ParentCategoryId == null)
            .ToListAsync();

        return _mapper.Map<IEnumerable<ExpenseCategoryListDto>>(mainCategories);
    }

    public async Task<IEnumerable<ExpenseCategoryListDto>> GetSubCategoriesAsync(int parentId)
    {
        var subCategories = await _unitOfWork.ExpenseCategories
            .Query()
            .Include(c => c.ParentCategory)
            .Include(c => c.Expenses)
            .Where(c => c.ParentCategoryId == parentId)
            .ToListAsync();

        return _mapper.Map<IEnumerable<ExpenseCategoryListDto>>(subCategories);
    }

    public async Task<bool> UpdateCategoryAsync(int id, ExpenseCategoryUpdateDto categoryDto)
    {
        ArgumentNullException.ThrowIfNull(categoryDto);

        var existingCategory = await _unitOfWork.ExpenseCategories.GetByIdAsync(id);
        if (existingCategory == null)
            return false;

        _mapper.Map(categoryDto, existingCategory);
        _unitOfWork.ExpenseCategories.Update(existingCategory);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteCategoryAsync(int id)
    {
        var category = await _unitOfWork.ExpenseCategories
            .Query()
            .Include(c => c.SubCategories)
            .Include(c => c.Expenses)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (category == null)
            return false;

        if (category.SubCategories?.Any() == true || category.Expenses?.Any() == true)
            throw new InvalidOperationException("Cannot delete category with subcategories or expenses");

        _unitOfWork.ExpenseCategories.Remove(category);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<decimal> GetTotalExpensesByCategoryAsync(int categoryId, DateTime? startDate, DateTime? endDate)
    {
        var query = _unitOfWork.Expenses
            .Query()
            .Where(e => e.ExpenseCategoryId == categoryId);

        if (startDate.HasValue)
            query = query.Where(e => e.ExpenseDate >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(e => e.ExpenseDate <= endDate.Value);

        return await query.SumAsync(e => e.Amount);
    }
    #endregion

    #region Reporting Operations
    public async Task<IEnumerable<ExpenseCategorySummaryDto>> GetExpensesSummaryByCategories(DateTime startDate, DateTime endDate)
    {
        var expenses = await _unitOfWork.Expenses
            .Query()
            .Include(e => e.Category)
            .Where(e => e.ExpenseDate >= startDate && e.ExpenseDate <= endDate)
            .GroupBy(e => new { e.Category.Id, e.Category.Name })
            .Select(g => new ExpenseCategorySummaryDto
            {
                CategoryId = g.Key.Id,
                CategoryName = g.Key.Name,
                TotalAmount = g.Sum(e => e.Amount),
                TransactionCount = g.Count(),
                PercentageOfTotal = 0
            })
            .ToListAsync();

        var totalAmount = expenses.Sum(e => e.TotalAmount);
        foreach (var expense in expenses)
        {
            expense.PercentageOfTotal = totalAmount > 0
                ? (expense.TotalAmount / totalAmount) * 100
                : 0;
        }

        return expenses;
    }

    public async Task<IEnumerable<DailyExpenseReportDto>> GetDailyExpensesAsync(DateTime startDate, DateTime endDate)
    {
        var dailyExpenses = await _unitOfWork.Expenses
            .Query()
            .Include(e => e.Category)
            .Where(e => e.ExpenseDate >= startDate && e.ExpenseDate <= endDate)
            .GroupBy(e => e.ExpenseDate.Date)
            .Select(g => new DailyExpenseReportDto
            {
                Date = g.Key,
                TotalAmount = g.Sum(e => e.Amount),
                CategoryBreakdown = g.GroupBy(e => e.Category.Name)
                    .ToDictionary(
                        cg => cg.Key,
                        cg => cg.Sum(e => e.Amount)
                    )
            })
            .OrderBy(x => x.Date)
            .ToListAsync();

        return dailyExpenses;
    }

    public async Task<bool> ApproveExpenseAsync(int id, string approvedBy)
    {
        var expense = await _unitOfWork.Expenses.GetByIdAsync(id);
        if (expense == null)
            return false;

        expense.IsApproved = true;
        expense.ApprovedBy = approvedBy;
        expense.ApprovalDate = DateTime.Now;

        _unitOfWork.Expenses.Update(expense);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
    #endregion
}