using _3dlogyERP.Core.Enums;

namespace _3dlogyERP.WebUI.Dtos.OrderDtos
{
    public class OrderListDto
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public string CustomerName { get; set; }
        public OrderStatus Status { get; set; }
        public string StatusText => Status.ToString();
        public OrderSource Source { get; set; }
        public string SourceText => Source.ToString();
        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public decimal TotalCost { get; set; }
        public decimal FinalPrice { get; set; }
        public bool RequiresShipping { get; set; }
        public string TrackingNumber { get; set; }
        public int ServiceCount { get; set; }
    }
}
