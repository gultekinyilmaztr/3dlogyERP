using System.Collections.Generic;
using System.Threading.Tasks;
using _3dlogyERP.Core.Entities;

namespace _3dlogyERP.Application.Services
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(Order order);
        Task<Order> GetOrderByIdAsync(int id);
        Task<Order> GetOrderByNumberAsync(string orderNumber);
        Task<Order> UpdateOrderAsync(Order order);
        Task<bool> DeleteOrderAsync(int id);
        Task<IEnumerable<Order>> GetCustomerOrdersAsync(int customerId);
        Task<Order> UpdateOrderStatusAsync(int orderId, OrderStatus status);
        Task<decimal> CalculateOrderTotalAsync(int orderId);
        Task<Order> ProcessNewOrderAsync(Order order);
        Task<bool> CancelOrderAsync(int orderId);
        Task<string> GenerateOrderNumberAsync();
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<IEnumerable<Order>> GetOrdersByStatusAsync(OrderStatus status);
        Task<decimal> CalculateOrderCostAsync(int orderId);
    }
}
