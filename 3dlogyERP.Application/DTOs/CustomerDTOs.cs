using System;
using System.Collections.Generic;

namespace _3dlogyERP.Application.DTOs
{
    // Müşterinin siparişlerini listelemek için kullanılacak DTO
    public class CustomerOrderListDTO
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public string Status { get; set; }
        public decimal FinalPrice { get; set; }
        public bool RequiresShipping { get; set; }
        public string TrackingNumber { get; set; }
        public List<CustomerOrderServiceDTO> Services { get; set; }
    }

    // Müşterinin sipariş detaylarını görmek için kullanılacak DTO
    public class CustomerOrderServiceDTO
    {
        public string ServiceTypeName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public TimeSpan EstimatedDuration { get; set; }
        public MaterialUsageDTO MaterialUsage { get; set; }
    }

    // Malzeme kullanım bilgileri için DTO
    public class MaterialUsageDTO
    {
        public string MaterialName { get; set; }
        public string MaterialTypeName { get; set; }
        public string Color { get; set; }
        public decimal Quantity { get; set; } // in grams
    }

    // Müşterinin profil bilgilerini görmek için kullanılacak DTO
    public class CustomerProfileDTO
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool IsPhoneVerified { get; set; }
        public string Address { get; set; }
    }
}
