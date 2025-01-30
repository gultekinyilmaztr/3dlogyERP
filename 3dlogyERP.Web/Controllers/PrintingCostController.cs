using _3dlogyERP.Application.Interfaces;
using _3dlogyERP.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace _3dlogyERP.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class PrintingCostController : ControllerBase
    {
        private readonly IPrintingCostService _printingCostService;
        private readonly ILogger<PrintingCostController> _logger;

        public PrintingCostController(
            IPrintingCostService printingCostService,
            ILogger<PrintingCostController> logger)
        {
            _printingCostService = printingCostService ?? throw new ArgumentNullException(nameof(printingCostService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost("calculate")]
        public async Task<ActionResult<PrintingCostCalculation>> CalculateCosts(PrintingCostCalculation input)
        {
            try
            {
                input.CreatedBy = User.Identity?.Name ?? "unknown";
                var result = await _printingCostService.CalculateCostsAsync(input);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "3D baskı maliyet hesaplama hatası");
                throw;
            }
        }

        [HttpPost]
        public async Task<ActionResult<PrintingCostCalculation>> SaveCalculation(PrintingCostCalculation calculation)
        {
            calculation.CreatedBy = User.Identity?.Name ?? "unknown";
            var result = await _printingCostService.SaveCalculationAsync(calculation);
            return CreatedAtAction(nameof(GetCalculation), new { id = result.Id }, result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PrintingCostCalculation>> GetCalculation(int id)
        {
            var calculation = await _printingCostService.GetCalculationByIdAsync(id);
            return Ok(calculation);
        }

        [HttpGet("user")]
        public async Task<ActionResult<IEnumerable<PrintingCostCalculation>>> GetUserCalculations()
        {
            var userId = User.Identity?.Name;
            if (string.IsNullOrEmpty(userId))
                return BadRequest("Kullanıcı kimliği bulunamadı");

            var calculations = await _printingCostService.GetCalculationsByUserAsync(userId);
            return Ok(calculations);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PrintingCostCalculation>>> GetAllCalculations()
        {
            var calculations = await _printingCostService.GetAllCalculationsAsync();
            return Ok(calculations);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCalculation(int id)
        {
            await _printingCostService.DeleteCalculationAsync(id);
            return NoContent();
        }
    }
}
