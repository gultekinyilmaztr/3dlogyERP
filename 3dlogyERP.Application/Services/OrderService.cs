using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using _3dlogyERP.Core.Entities;
using _3dlogyERP.Core.Interfaces;
using _3dlogyERP.Application.DTOs;

namespace _3dlogyERP.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            order.CreatedAt = DateTime.UtcNow;
            order.UpdatedAt = DateTime.UtcNow;
            order.Status = OrderStatus.New;
            order.OrderNumber = await GenerateOrderNumberAsync();

            await _unitOfWork.Orders.AddAsync(order);
            await _unitOfWork.SaveChangesAsync();

            return order;
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            return await _unitOfWork.Orders.GetByIdAsync(id);
        }

        public async Task<Order> GetOrderByNumberAsync(string orderNumber)
        {
            if (string.IsNullOrEmpty(orderNumber))
                throw new ArgumentNullException(nameof(orderNumber));

            var orders = await _unitOfWork.Orders.FindAsync(o => o.OrderNumber == orderNumber);
            return orders.FirstOrDefault();
        }

        public async Task<Order> UpdateOrderAsync(Order order)
        {
            var existingOrder = await _unitOfWork.Orders.GetByIdAsync(order.Id);
            if (existingOrder == null)
                return null;

            existingOrder.Status = order.Status;
            existingOrder.UpdatedAt = DateTime.UtcNow;
            
            _unitOfWork.Orders.Update(existingOrder);
            await _unitOfWork.SaveChangesAsync();

            return existingOrder;
        }

        public async Task<bool> DeleteOrderAsync(int id)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(id);
            if (order == null)
                return false;

            _unitOfWork.Orders.Remove(order);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<Order>> GetCustomerOrdersAsync(int customerId)
        {
            return await _unitOfWork.Orders.FindAsync(o => o.CustomerId == customerId);
        }

        public async Task<Order> UpdateOrderStatusAsync(int orderId, OrderStatus status)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(orderId);
            if (order == null)
                return null;

            order.Status = status;
            order.UpdatedAt = DateTime.UtcNow;

            if (status == OrderStatus.Completed)
            {
                order.CompletedAt = DateTime.UtcNow;
            }

            _unitOfWork.Orders.Update(order);
            await _unitOfWork.SaveChangesAsync();

            return order;
        }

        public async Task<decimal> CalculateOrderTotalAsync(int orderId)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(orderId);
            if (order == null)
                return 0;

            var services = await _unitOfWork.OrderServices.FindAsync(s => s.OrderId == orderId);
            return services.Sum(s => s.TotalCost);
        }

        public async Task<Order> ProcessNewOrderAsync(Order order)
        {
            order.Status = OrderStatus.New;
            order.CreatedAt = DateTime.UtcNow;
            order.UpdatedAt = DateTime.UtcNow;
            order.OrderNumber = await GenerateOrderNumberAsync();

            await _unitOfWork.Orders.AddAsync(order);
            await _unitOfWork.SaveChangesAsync();

            return order;
        }

        public async Task<bool> CancelOrderAsync(int orderId)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(orderId);
            if (order == null)
                return false;

            order.Status = OrderStatus.Cancelled;
            order.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<string> GenerateOrderNumberAsync()
        {
            var lastOrder = await _unitOfWork.Orders
                .FindAsync(o => true)
                .ContinueWith(t => t.Result.OrderByDescending(x => x.CreatedAt).FirstOrDefault());

            int orderNumber = 1;
            if (lastOrder != null)
            {
                string lastOrderNumber = lastOrder.OrderNumber;
                if (int.TryParse(lastOrderNumber.Substring(8), out int lastNumber))
                {
                    orderNumber = lastNumber + 1;
                }
            }

            return $"ORD{DateTime.UtcNow:yyyyMMdd}{orderNumber:D5}";
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _unitOfWork.Orders.GetAllAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersByStatusAsync(OrderStatus status)
        {
            return await _unitOfWork.Orders.FindAsync(o => o.Status == status);
        }

        public async Task<decimal> CalculateOrderCostAsync(int orderId)
        {
            var order = await GetOrderByIdAsync(orderId);
            if (order == null)
                return 0;

            decimal totalCost = 0;
            foreach (var service in order.Services)
            {
                // Calculate equipment costs
                if (service.Equipment != null)
                {
                    var equipmentHours = (service.EndTime - service.StartTime)?.TotalHours ?? 0;
                    totalCost += (decimal)equipmentHours * (service.Equipment.HourlyRate + service.Equipment.MaintenanceCostPerHour + service.Equipment.ElectricityConsumptionPerHour);
                }

                // Calculate material costs
                if (service.Material != null && service.MaterialQuantity > 0)
                {
                    totalCost += service.MaterialQuantity * service.Material.CostPerKg;
                }

                // Add service base price
                totalCost += service.Price;
            }

            return totalCost;
        }
    }
}
