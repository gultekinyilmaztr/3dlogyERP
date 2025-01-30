using System.ComponentModel;

namespace _3dlogyERP.Core.Enums
{
    public enum StockCategory
    {
        [Description("HM")]
        Hammadde = 1,

        [Description("SM")]
        SarfMalzeme = 2,

        [Description("M")]
        Mamul = 3,

        [Description("SK")]
        SabitKiymet = 4
    }

    // Enum extension method
    public static class StockCategoryExtensions
    {
        public static string GetDescription(this StockCategory value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
            return attribute?.Description ?? value.ToString();
        }
    }
}
