namespace _3dlogyERP.Core.Entities
{
    public class ServiceType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal BasePrice { get; set; }
        public bool IsActive { get; set; }

        // Navigation property
        public virtual ICollection<OrderService> OrderServices { get; set; }
    }
}
