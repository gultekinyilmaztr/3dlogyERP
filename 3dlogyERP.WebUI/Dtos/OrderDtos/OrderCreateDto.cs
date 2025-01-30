using _3dlogyERP.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace _3dlogyERP.WebUI.Dtos.OrderDtos
{
    public class OrderCreateDto
    {
        [Required(ErrorMessage = "Sipariş numarası zorunludur")]
        public string OrderNumber { get; set; }

        [Required(ErrorMessage = "Müşteri seçimi zorunludur")]
        public int CustomerId { get; set; }

        public OrderStatus Status { get; set; } = OrderStatus.New;

        [Required(ErrorMessage = "Sipariş kaynağı zorunludur")]
        public OrderSource Source { get; set; }

        public decimal TotalCost { get; set; }
        public decimal FinalPrice { get; set; }
        public string Notes { get; set; }
        public bool RequiresShipping { get; set; }

        [Required(ErrorMessage = "Kargo gerekli ise teslimat adresi zorunludur")]
        public string ShippingAddress { get; set; }
    }
}
