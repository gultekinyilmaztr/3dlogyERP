using _3dlogyERP.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace _3dlogyERP.WebUI.Dtos.OrderDtos
{
    public class OrderUpdateDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Sipariş numarası zorunludur")]
        public string OrderNumber { get; set; }

        [Required(ErrorMessage = "Müşteri seçimi zorunludur")]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Sipariş durumu zorunludur")]
        public OrderStatus Status { get; set; }

        [Required(ErrorMessage = "Sipariş kaynağı zorunludur")]
        public OrderSource Source { get; set; }

        public decimal TotalCost { get; set; }
        public decimal FinalPrice { get; set; }
        public string Notes { get; set; }
        public bool RequiresShipping { get; set; }
        public string ShippingAddress { get; set; }
        public string TrackingNumber { get; set; }
    }
}
