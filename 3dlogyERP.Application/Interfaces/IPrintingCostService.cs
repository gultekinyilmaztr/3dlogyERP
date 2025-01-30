
using _3dlogyERP.Core.Entities;

namespace _3dlogyERP.Application.Interfaces
{
    public interface IPrintingCostService
    {
        Task<PrintingCostCalculation> CalculateCostsAsync(PrintingCostCalculation input);
        Task<PrintingCostCalculation> SaveCalculationAsync(PrintingCostCalculation calculation);
        Task<PrintingCostCalculation> GetCalculationByIdAsync(int id);
        Task<IEnumerable<PrintingCostCalculation>> GetCalculationsByUserAsync(string userId);
        Task<IEnumerable<PrintingCostCalculation>> GetAllCalculationsAsync();
        Task DeleteCalculationAsync(int id);
    }
}
