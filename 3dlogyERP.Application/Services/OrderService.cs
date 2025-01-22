using _3dlogyERP.Core.Entities;
using _3dlogyERP.Core.Enums;
using _3dlogyERP.Core.Interfaces;

namespace _3dlogyERP.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IUnitOfWork unitOfWork)
        {
            ArgumentNullException.ThrowIfNull(unitOfWork);
            
            _unitOfWork = unitOfWork;
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            ArgumentNullException.ThrowIfNull(order);
            
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
            ArgumentNullException.ThrowIfNull(orderNumber);
            
            if (string.IsNullOrEmpty(orderNumber))
                throw new ArgumentNullException(nameof(orderNumber));

            var orders = await _unitOfWork.Orders.FindAsync(o => o.OrderNumber == orderNumber);
            return orders.FirstOrDefault();
        }

        public async Task<Order> UpdateOrderAsync(Order order)
        {
            ArgumentNullException.ThrowIfNull(order);
            
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
            ArgumentNullException.ThrowIfNull(order);
            
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            // Validate customer exists
            var customer = await _unitOfWork.Customers.GetByIdAsync(order.CustomerId);
            if (customer == null)
                throw new InvalidOperationException("Müşteri bulunamadı.");

            // Initialize order properties
            order.Status = OrderStatus.New;
            order.CreatedAt = DateTime.UtcNow;
            order.UpdatedAt = DateTime.UtcNow;
            order.OrderNumber = await GenerateOrderNumberAsync();

            // Validate and process services
            if (order.Services != null && order.Services.Any())
            {
                foreach (var service in order.Services)
                {
                    // Validate service type
                    if (service.ServiceTypeId <= 0)
                        throw new InvalidOperationException("Geçersiz servis tipi.");

                    var serviceType = await _unitOfWork.ServiceTypes.GetByIdAsync(service.ServiceTypeId);
                    if (serviceType == null)
                        throw new InvalidOperationException($"Servis tipi bulunamadı: {service.ServiceTypeId}");

                    // Set initial service status
                    service.Status = ServiceStatus.Pending;
                    service.CreatedAt = DateTime.UtcNow;

                    // Validate equipment if specified
                    if (service.EquipmentId.HasValue)
                    {
                        var equipment = await _unitOfWork.Equipment.GetByIdAsync(service.EquipmentId.Value);
                        if (equipment == null)
                            throw new InvalidOperationException($"Ekipman bulunamadı: {service.EquipmentId}");

                        if (!equipment.IsAvailable)
                            throw new InvalidOperationException($"Ekipman müsait değil: {equipment.Name}");
                    }

                    // Validate material if specified
                    if (service.MaterialId.HasValue)
                    {
                        var material = await _unitOfWork.Materials.GetByIdAsync(service.MaterialId.Value);
                        if (material == null)
                            throw new InvalidOperationException($"Malzeme bulunamadı: {service.MaterialId}");

                        if (material.StockQuantity < service.MaterialQuantity)
                            throw new InvalidOperationException($"Yetersiz stok. Mevcut: {material.StockQuantity}, İstenen: {service.MaterialQuantity}");
                    }
                }
            }
            else
            {
                throw new InvalidOperationException("Sipariş en az bir servis içermelidir.");
            }

            // Calculate initial cost
            order.FinalPrice = await CalculateOrderCostAsync(order.Id);

            // Save order
            await _unitOfWork.Orders.AddAsync(order);
            await _unitOfWork.SaveChangesAsync();

            // Return the created order
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
            var today = DateTime.UtcNow.Date;
            var tomorrow = today.AddDays(1);

            // Get all orders from today to ensure proper sequence
            var todaysOrders = await _unitOfWork.Orders
                .FindAsync(o => o.CreatedAt >= today && o.CreatedAt < tomorrow);

            int sequence = 1;
            if (todaysOrders.Any())
            {
                // Extract the sequence numbers from today's orders
                var sequences = todaysOrders
                    .Select(o => int.TryParse(o.OrderNumber.Substring(8), out int seq) ? seq : 0)
                    .Where(seq => seq > 0);

                // Get the highest sequence number and increment
                if (sequences.Any())
                {
                    sequence = sequences.Max() + 1;
                }
            }

            // Format: ORD + YYYYMMDD + 5-digit sequence
            string orderNumber = $"ORD{today:yyyyMMdd}{sequence:D5}";

            // Verify uniqueness (in case of concurrent operations)
            while (await GetOrderByNumberAsync(orderNumber) != null)
            {
                sequence++;
                orderNumber = $"ORD{today:yyyyMMdd}{sequence:D5}";
            }

            return orderNumber;
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
