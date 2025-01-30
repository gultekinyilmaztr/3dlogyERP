using _3dlogyERP.Core.Enums;

namespace _3dlogyERP.Core.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public int CustomerId { get; set; }
        public OrderStatus Status { get; set; }
        public OrderSource Source { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public decimal TotalCost { get; set; }
        public decimal FinalPrice { get; set; }
        public string Notes { get; set; }
        public bool RequiresShipping { get; set; }
        public string ShippingAddress { get; set; }
        public string TrackingNumber { get; set; }

        // Navigation properties
        public virtual Customer Customer { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public virtual ICollection<OrderService> Services { get; set; } = new List<OrderService>();
        public virtual ICollection<OrderDocument> Documents { get; set; } = new List<OrderDocument>();
    }
}