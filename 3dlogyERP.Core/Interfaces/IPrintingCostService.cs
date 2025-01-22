using _3dlogyERP.Core.Entities.PrintingCost;

namespace _3dlogyERP.Core.Interfaces
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
