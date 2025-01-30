namespace _3dlogyERP.Core.Entities
{
    public class PrintingCostCalculation
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string ProjectName { get; set; } = string.Empty;

        // Malzeme bilgileri
        public int MaterialId { get; set; }
        public Material? Material { get; set; }
        public decimal MaterialUsed { get; set; }       // gr

        // Baskı parametreleri
        public decimal PrintingTime { get; set; }       // saat
        public decimal PowerConsumption { get; set; }   // Watt
        public decimal ElectricityRate { get; set; }    // kWh başına TL
        public decimal PrinterCost { get; set; }        // TL
        public decimal PreparationTime { get; set; }    // saat
        public decimal LaborRate { get; set; }          // saat başına TL
        public decimal AdditionalCosts { get; set; }    // TL
        public decimal ProfitMargin { get; set; }       // % olarak (örn: 20 = %20)

        // Baskı ayarları
        public decimal HotendTemperature { get; set; }  // °C
        public decimal BedTemperature { get; set; }     // °C
        public decimal PrintSpeed { get; set; }         // mm/s

        // Hesaplanan değerler
        public decimal MaterialCost { get; set; }
        public decimal ElectricityCost { get; set; }
        public decimal DepreciationCost { get; set; }
        public decimal LaborCost { get; set; }
        public decimal TotalCost { get; set; }
        public decimal SuggestedPrice { get; set; }

        // Metadata
        public DateTime CalculationDate { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public bool IsSuccessful { get; set; }         // Baskı başarılı mı?
        public string? FailureReason { get; set; }     // Başarısız olduysa nedeni
    }
}
