using System;
using System.Collections.Generic;

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
        public virtual ICollection<OrderService> Services { get; set; }
        public virtual ICollection<OrderDocument> Documents { get; set; }
    }

    public enum OrderStatus
    {
        New,
        Quoted,
        Approved,
        InProgress,
        ReadyForDelivery,
        Shipped,
        Completed,
        Cancelled
    }

    public enum OrderSource
    {
        WebPortal,
        Email,
        Phone,
        SocialMedia,
        Direct
    }
}
