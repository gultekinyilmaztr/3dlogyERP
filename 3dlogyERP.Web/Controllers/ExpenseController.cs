using _3dlogyERP.Application.Dtos.ExpenseCategoryDtos;
using _3dlogyERP.Application.Dtos.ExpenseDtos;
using _3dlogyERP.Application.Interfaces;
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

        #region Expense Operations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExpenseListDto>>> GetAllExpenses()
        {
            try
            {
                var expenses = await _expenseService.GetAllExpensesAsync();
                return Ok(expenses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all expenses");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ExpenseListDto>> GetExpenseById(int id)
        {
            try
            {
                var expense = await _expenseService.GetExpenseByIdAsync(id);
                if (expense == null)
                    return NotFound();

                return Ok(expense);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting expense with ID: {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<ExpenseListDto>> CreateExpense([FromBody] ExpenseCreateDto expenseDto)
        {
            try
            {
                var result = await _expenseService.CreateExpenseAsync(expenseDto);
                return CreatedAtAction(nameof(GetExpenseById), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating expense");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateExpense(int id, [FromBody] ExpenseUpdateDto expenseDto)
        {
            try
            {
                var result = await _expenseService.UpdateExpenseAsync(id, expenseDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating expense with ID: {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteExpense(int id)
        {
            try
            {
                var result = await _expenseService.DeleteExpenseAsync(id);
                if (!result)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting expense with ID: {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("{id:int}/approve")]
        public async Task<IActionResult> ApproveExpense(int id, [FromBody] string approvedBy)
        {
            try
            {
                var result = await _expenseService.ApproveExpenseAsync(id, approvedBy);
                if (!result)
                    return NotFound();

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error approving expense with ID: {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }
        #endregion

        #region Category Operations
        [HttpGet("categories")]
        public async Task<ActionResult<IEnumerable<ExpenseCategoryListDto>>> GetAllCategories()
        {
            try
            {
                var categories = await _expenseService.GetAllCategoriesAsync();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all categories");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("categories/{id:int}")]
        public async Task<ActionResult<ExpenseCategoryDetailDto>> GetCategoryById(int id)
        {
            try
            {
                var category = await _expenseService.GetCategoryByIdAsync(id);
                return Ok(category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting category with ID: {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("categories/main")]
        public async Task<ActionResult<IEnumerable<ExpenseCategoryListDto>>> GetMainCategories()
        {
            try
            {
                var categories = await _expenseService.GetMainCategoriesAsync();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting main categories");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("categories/{parentId:int}/subcategories")]
        public async Task<ActionResult<IEnumerable<ExpenseCategoryListDto>>> GetSubCategories(int parentId)
        {
            try
            {
                var categories = await _expenseService.GetSubCategoriesAsync(parentId);
                return Ok(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting subcategories for parent ID: {ParentId}", parentId);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("categories")]
        public async Task<ActionResult<ExpenseCategoryDetailDto>> CreateCategory([FromBody] ExpenseCategoryCreateDto categoryDto)
        {
            try
            {
                var result = await _expenseService.CreateCategoryAsync(categoryDto);
                return CreatedAtAction(nameof(GetCategoryById), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating category");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("categories/{id:int}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] ExpenseCategoryUpdateDto categoryDto)
        {
            try
            {
                var result = await _expenseService.UpdateCategoryAsync(id, categoryDto);
                if (!result)
                    return NotFound();

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating category with ID: {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("categories/{id:int}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                var result = await _expenseService.DeleteCategoryAsync(id);
                if (!result)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting category with ID: {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }
        #endregion

        #region Reporting Operations
        [HttpGet("reports/by-category")]
        public async Task<ActionResult<IEnumerable<ExpenseCategorySummaryDto>>> GetExpensesSummaryByCategories(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            try
            {
                var summary = await _expenseService.GetExpensesSummaryByCategories(startDate, endDate);
                return Ok(summary);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting expenses summary by categories");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("reports/daily")]
        public async Task<ActionResult<IEnumerable<DailyExpenseReportDto>>> GetDailyExpenses(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            try
            {
                var report = await _expenseService.GetDailyExpensesAsync(startDate, endDate);
                return Ok(report);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting daily expenses report");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("categories/{categoryId:int}/total")]
        public async Task<ActionResult<decimal>> GetTotalExpensesByCategory(
            int categoryId,
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate)
        {
            try
            {
                var total = await _expenseService.GetTotalExpensesByCategoryAsync(categoryId, startDate, endDate);
                return Ok(total);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting total expenses for category ID: {CategoryId}", categoryId);
                return StatusCode(500, "Internal server error");
            }
        }
        #endregion
    }
}