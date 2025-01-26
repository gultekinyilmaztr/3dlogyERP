using _3dlogyERP.Core.Entities;
using _3dlogyERP.Core.Entities.PrintingCost;
using _3dlogyERP.Core.Exceptions;
using _3dlogyERP.Core.Interfaces;

namespace _3dlogyERP.Application.Services
{
    public class PrintingCostService : IPrintingCostService
    {
        private readonly IRepository<PrintingCostCalculation> _repository;
        private readonly IRepository<Material> _materialRepository;
        private const decimal HOURS_PER_YEAR = 8760m; // 365 gün * 24 saat
        private const decimal PRINTER_LIFETIME_YEARS = 5m; // Yazıcının tahmini ömrü

        public PrintingCostService(
            IRepository<PrintingCostCalculation> repository,
            IRepository<Material> materialRepository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _materialRepository = materialRepository ?? throw new ArgumentNullException(nameof(materialRepository));
        }

        public async Task<PrintingCostCalculation> CalculateCostsAsync(PrintingCostCalculation input)
        {
            var material = await _materialRepository.GetByIdAsync(input.MaterialId);
            if (material == null)
                throw new NotFoundException($"Malzeme bulunamadı: {input.MaterialId}");

            input.Material = material;
            ValidateInput(input);

            // Malzeme maliyeti
            input.MaterialCost = CalculateMaterialCost(material.UnitCost, input.MaterialUsed);

            // Elektrik maliyeti
            input.ElectricityCost = CalculateElectricityCost(
                input.PrintingTime,
                input.PowerConsumption,
                input.ElectricityRate);

            // Amortisman
            input.DepreciationCost = CalculateDepreciationCost(
                input.PrinterCost,
                input.PrintingTime);

            // İşçilik maliyeti
            input.LaborCost = CalculateLaborCost(
                input.PrintingTime,
                input.PreparationTime,
                input.LaborRate);

            // Toplam maliyet
            input.TotalCost = CalculateTotalCost(
                input.MaterialCost,
                input.ElectricityCost,
                input.DepreciationCost,
                input.LaborCost,
                input.AdditionalCosts);

            // Önerilen satış fiyatı
            input.SuggestedPrice = CalculateSuggestedPrice(
                input.TotalCost,
                input.ProfitMargin);

            input.CalculationDate = DateTime.UtcNow;

            return input;
        }

        private void ValidateInput(PrintingCostCalculation input)
        {
            if (input.Material == null || input.Material.UnitCost <= 0)
                throw new ValidationException("Geçerli bir malzeme ve birim fiyat gerekli.");

            if (input.MaterialUsed <= 0)
                throw new ValidationException("Kullanılan malzeme miktarı 0'dan büyük olmalıdır.");

            if (input.PrintingTime <= 0)
                throw new ValidationException("Baskı süresi 0'dan büyük olmalıdır.");

            if (input.PowerConsumption <= 0)
                throw new ValidationException("Güç tüketimi 0'dan büyük olmalıdır.");

            if (input.ElectricityRate <= 0)
                throw new ValidationException("Elektrik birim fiyatı 0'dan büyük olmalıdır.");

            if (input.PrinterCost <= 0)
                throw new ValidationException("Yazıcı maliyeti 0'dan büyük olmalıdır.");

            if (input.LaborRate <= 0)
                throw new ValidationException("İşçilik ücreti 0'dan büyük olmalıdır.");

            if (input.ProfitMargin < 0)
                throw new ValidationException("Kar marjı 0'dan küçük olamaz.");
        }

        private decimal CalculateMaterialCost(decimal unitPrice, decimal amount)
        {
            return Math.Round(unitPrice * amount, 4);
        }

        private decimal CalculateElectricityCost(decimal hours, decimal watts, decimal rate)
        {
            decimal kWh = (watts * hours) / 1000m;
            return Math.Round(kWh * rate, 4);
        }

        private decimal CalculateDepreciationCost(decimal printerCost, decimal hours)
        {
            decimal hourlyDepreciation = printerCost / (HOURS_PER_YEAR * PRINTER_LIFETIME_YEARS);
            return Math.Round(hourlyDepreciation * hours, 4);
        }

        private decimal CalculateLaborCost(decimal printingTime, decimal prepTime, decimal rate)
        {
            return Math.Round((printingTime + prepTime) * rate, 4);
        }

        private decimal CalculateTotalCost(decimal materialCost, decimal electricityCost,
            decimal depreciation, decimal labor, decimal additional)
        {
            return Math.Round(materialCost + electricityCost + depreciation + labor + additional, 4);
        }

        private decimal CalculateSuggestedPrice(decimal totalCost, decimal profitMargin)
        {
            decimal markup = 1m + (profitMargin / 100m);
            return Math.Round(totalCost * markup, 4);
        }

        public async Task<PrintingCostCalculation> SaveCalculationAsync(PrintingCostCalculation calculation)
        {
            await _repository.AddAsync(calculation);
            return calculation;
        }

        public async Task<PrintingCostCalculation> GetCalculationByIdAsync(int id)
        {
            var calculation = await _repository.GetByIdAsync(id);
            if (calculation == null)
                throw new NotFoundException($"Hesaplama bulunamadı: {id}");
            return calculation;
        }

        public async Task<IEnumerable<PrintingCostCalculation>> GetCalculationsByUserAsync(string userId)
        {
            return await _repository.FindAsync(c => c.UserId == userId);
        }

        public async Task<IEnumerable<PrintingCostCalculation>> GetAllCalculationsAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task DeleteCalculationAsync(int id)
        {
            var calculation = await GetCalculationByIdAsync(id);
            _repository.Remove(calculation);
        }
    }
}
