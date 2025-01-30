using _3dlogyERP.Application.Dtos.OrderDtos;
using _3dlogyERP.Application.Interfaces;
using _3dlogyERP.Core.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OrderStatus = _3dlogyERP.Core.Enums.OrderStatus;

namespace _3dlogyERP.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            ArgumentNullException.ThrowIfNull(unitOfWork);
            ArgumentNullException.ThrowIfNull(mapper);

            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<OrderListDto> CreateOrderAsync(OrderCreateDto orderDto)
        {
            ArgumentNullException.ThrowIfNull(orderDto);

            var order = _mapper.Map<Order>(orderDto);
            order.CreatedAt = DateTime.UtcNow;
            order.UpdatedAt = DateTime.UtcNow;
            order.Status = OrderStatus.New;
            order.OrderNumber = await GenerateOrderNumberAsync();

            await _unitOfWork.Orders.AddAsync(order);
            await _unitOfWork.SaveChangesAsync();

            return await GetOrderByIdAsync(order.Id);
        }

        public async Task<OrderListDto> GetOrderByIdAsync(int id)
        {
            var order = await _unitOfWork.Orders
                .Query()
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == id);

            return _mapper.Map<OrderListDto>(order);
        }

        public async Task<OrderListDto> GetOrderByNumberAsync(string orderNumber)
        {
            ArgumentNullException.ThrowIfNull(orderNumber);

            var order = await _unitOfWork.Orders
                .Query()
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.OrderNumber == orderNumber);

            return _mapper.Map<OrderListDto>(order);
        }

        public async Task<OrderListDto> UpdateOrderAsync(int id, OrderUpdateDto orderDto)
        {
            ArgumentNullException.ThrowIfNull(orderDto);

            var existingOrder = await _unitOfWork.Orders.GetByIdAsync(id);
            if (existingOrder == null)
                return null;

            _mapper.Map(orderDto, existingOrder);
            existingOrder.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Orders.Update(existingOrder);
            await _unitOfWork.SaveChangesAsync();

            return await GetOrderByIdAsync(id);
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

        public async Task<IEnumerable<OrderListDto>> GetCustomerOrdersAsync(int customerId)
        {
            var orders = await _unitOfWork.Orders
                .Query()
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                .Where(o => o.CustomerId == customerId)
                .ToListAsync();

            return _mapper.Map<IEnumerable<OrderListDto>>(orders);
        }

        public async Task<OrderListDto> UpdateOrderStatusAsync(int orderId, OrderStatus status)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(orderId);
            if (order == null)
                return null;

            order.Status = status;
            order.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Orders.Update(order);
            await _unitOfWork.SaveChangesAsync();

            return await GetOrderByIdAsync(orderId);
        }

        public async Task<decimal> CalculateOrderTotalAsync(int orderId)
        {
            var order = await _unitOfWork.Orders
                .Query()
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
                return 0;

            return order.OrderItems.Sum(item => item.Quantity * item.UnitPrice);
        }

        public async Task<OrderListDto> ProcessNewOrderAsync(OrderCreateDto orderDto)
        {
            var order = await CreateOrderAsync(orderDto);
            if (order != null)
            {
                await UpdateOrderStatusAsync(order.Id, OrderStatus.Processing);
                // Additional processing logic here
            }
            return order;
        }

        public async Task<bool> CancelOrderAsync(int orderId)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(orderId);
            if (order == null || order.Status == OrderStatus.Completed)
                return false;

            order.Status = OrderStatus.Cancelled;
            order.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Orders.Update(order);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<string> GenerateOrderNumberAsync()
        {
            var date = DateTime.UtcNow.ToString("yyyyMMdd");
            var lastOrder = await _unitOfWork.Orders
                .Query()
                .Where(o => o.OrderNumber.StartsWith(date))
                .OrderByDescending(o => o.OrderNumber)
                .FirstOrDefaultAsync();

            int sequence = 1;
            if (lastOrder != null)
            {
                var lastSequence = int.Parse(lastOrder.OrderNumber.Substring(8));
                sequence = lastSequence + 1;
            }

            return $"{date}{sequence:D4}";
        }

        public async Task<IEnumerable<OrderListDto>> GetAllOrdersAsync()
        {
            var orders = await _unitOfWork.Orders
                .Query()
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                .ToListAsync();

            return _mapper.Map<IEnumerable<OrderListDto>>(orders);
        }

        public async Task<IEnumerable<OrderListDto>> GetOrdersByStatusAsync(OrderStatus status)
        {
            var orders = await _unitOfWork.Orders
                .Query()
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                .Where(o => o.Status == status)
                .ToListAsync();

            return _mapper.Map<IEnumerable<OrderListDto>>(orders);
        }

        public async Task<decimal> CalculateOrderCostAsync(int orderId)
        {
            var order = await _unitOfWork.Orders
                .Query()
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
                return 0;

            decimal totalCost = 0;
            foreach (var item in order.OrderItems)
            {
                // Maliyet hesaplama mantığı burada uygulanacak
                var itemCost = item.Quantity * item.UnitCost; // UnitCost eklenmeli
                totalCost += itemCost;
            }

            return totalCost;
        }

        public async Task<bool> ExistsByIdAsync(int id)
        {
            return await _unitOfWork.Orders
                .Query()
                .AnyAsync(o => o.Id == id);
        }
    }
}