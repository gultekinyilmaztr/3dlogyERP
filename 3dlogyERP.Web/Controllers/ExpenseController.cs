using _3dlogyERP.Core.Entities;
using _3dlogyERP.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace _3dlogyERP.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseService _expenseService;
        private readonly ILogger<ExpenseController> _logger;

        public ExpenseController(IExpenseService expenseService, ILogger<ExpenseController> logger)
        {
            ArgumentNullException.ThrowIfNull(expenseService);
            ArgumentNullException.ThrowIfNull(logger);
            
            _expenseService = expenseService;
            _logger = logger;
        }

        // Harcama İşlemleri
        [HttpPost]
        public async Task<IActionResult> CreateExpense([FromBody] Expense expense)
        {
            var result = await _expenseService.CreateExpenseAsync(expense);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetExpense(int id)
        {
            var expense = await _expenseService.GetExpenseByIdAsync(id);
            if (expense == null)
                return NotFound();
            return Ok(expense);
        }

        [HttpGet]
        [Route("expenses")]
        public async Task<IActionResult> GetAllExpenses()
        {
            var expenses = await _expenseService.GetAllExpensesAsync();
            return Ok(expenses);
        }

        [HttpGet("bydate")]
        public async Task<IActionResult> GetExpensesByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var expenses = await _expenseService.GetExpensesByDateRangeAsync(startDate, endDate);
            return Ok(expenses);
        }

        [HttpGet("by-category/{categoryId:int}")]
        public async Task<IActionResult> GetExpensesByCategory(int categoryId)
        {
            var expenses = await _expenseService.GetExpensesByCategoryAsync(categoryId);
            return Ok(expenses);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateExpense(int id, [FromBody] Expense expense)
        {
            if (id != expense.Id)
                return BadRequest();

            var result = await _expenseService.UpdateExpenseAsync(expense);
            if (!result)
                return NotFound();
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteExpense(int id)
        {
            var result = await _expenseService.DeleteExpenseAsync(id);
            if (!result)
                return NotFound();
            return Ok();
        }

        [HttpPost("{id:int}/approve")]
        public async Task<IActionResult> ApproveExpense(int id, [FromQuery] string approvedBy)
        {
            var result = await _expenseService.ApproveExpenseAsync(id, approvedBy);
            if (!result)
                return NotFound();
            return Ok();
        }

        // Kategori İşlemleri
        [HttpPost("category")]
        public async Task<IActionResult> CreateCategory([FromBody] ExpenseCategory category)
        {
            var result = await _expenseService.CreateCategoryAsync(category);
            return Ok(result);
        }

        [HttpGet("category/{id:int}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var category = await _expenseService.GetCategoryByIdAsync(id);
            if (category == null)
                return NotFound();
            return Ok(category);
        }

        [HttpGet("categories")]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _expenseService.GetAllCategoriesAsync();
            return Ok(categories);
        }

        [HttpGet("categories/main")]
        public async Task<IActionResult> GetMainCategories()
        {
            var categories = await _expenseService.GetMainCategoriesAsync();
            return Ok(categories);
        }

        [HttpGet("categories/{parentId:int}/sub")]
        public async Task<IActionResult> GetSubCategories(int parentId)
        {
            var categories = await _expenseService.GetSubCategoriesAsync(parentId);
            return Ok(categories);
        }

        [HttpPut("category/{id:int}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] ExpenseCategory category)
        {
            if (id != category.Id)
                return BadRequest();

            var result = await _expenseService.UpdateCategoryAsync(category);
            if (!result)
                return NotFound();
            return Ok();
        }

        [HttpDelete("category/{id:int}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var result = await _expenseService.DeleteCategoryAsync(id);
            if (!result)
                return NotFound();
            return Ok();
        }

        // Raporlama İşlemleri
        [HttpGet("reports/total")]
        public async Task<IActionResult> GetTotalExpenses([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var total = await _expenseService.GetTotalExpensesByDateRangeAsync(startDate, endDate);
            return Ok(total);
        }

        [HttpGet("reports/category/{categoryId:int}/total")]
        public async Task<IActionResult> GetTotalExpensesByCategory(int categoryId, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            var total = await _expenseService.GetTotalExpensesByCategoryAsync(categoryId, startDate, endDate);
            return Ok(total);
        }

        [HttpGet("reports/summary")]
        public async Task<IActionResult> GetExpensesSummary([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var summary = await _expenseService.GetExpensesSummaryByCategories(startDate, endDate);
            return Ok(summary);
        }

        [HttpGet("reports/daily")]
        public async Task<IActionResult> GetDailyExpenses([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var dailyExpenses = await _expenseService.GetDailyExpensesAsync(startDate, endDate);
            return Ok(dailyExpenses);
        }
    }
}
