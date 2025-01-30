using _3dlogyERP.Application.Dtos.OrderDtos;
using _3dlogyERP.Core.Enums;

namespace _3dlogyERP.Application.Interfaces
{
    public interface IOrderService
    {
        Task<OrderListDto> CreateOrderAsync(OrderCreateDto orderDto);
        Task<OrderListDto> GetOrderByIdAsync(int id);
        Task<OrderListDto> GetOrderByNumberAsync(string orderNumber);
        Task<OrderListDto> UpdateOrderAsync(int id, OrderUpdateDto orderDto);
        Task<bool> DeleteOrderAsync(int id);
        Task<IEnumerable<OrderListDto>> GetCustomerOrdersAsync(int customerId);
        Task<OrderListDto> UpdateOrderStatusAsync(int orderId, OrderStatus status);
        Task<decimal> CalculateOrderTotalAsync(int orderId);
        Task<OrderListDto> ProcessNewOrderAsync(OrderCreateDto orderDto);
        Task<bool> CancelOrderAsync(int orderId);
        Task<string> GenerateOrderNumberAsync();
        Task<IEnumerable<OrderListDto>> GetAllOrdersAsync();
        Task<IEnumerable<OrderListDto>> GetOrdersByStatusAsync(OrderStatus status);
        Task<decimal> CalculateOrderCostAsync(int orderId);
        Task<bool> ExistsByIdAsync(int id);
    }
}
