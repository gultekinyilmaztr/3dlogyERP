using _3dlogyERP.Application.DTOs;
using _3dlogyERP.Application.Services;
using _3dlogyERP.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace _3dlogyERP.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetOrders()
        {
            if (User.IsInRole(UserRoles.Customer))
            {
                var customerId = int.Parse(User.FindFirst("CustomerId")?.Value ?? "0");
                var orders = await _orderService.GetCustomerOrdersAsync(customerId);
                var orderDtos = orders.Select(order => new CustomerOrderListDTO
                {
                    Id = order.Id,
                    OrderNumber = order.OrderNumber,
                    CreatedAt = order.CreatedAt,
                    CompletedAt = order.CompletedAt,
                    Status = order.Status.ToString(),
                    FinalPrice = order.FinalPrice,
                    RequiresShipping = order.RequiresShipping,
                    TrackingNumber = order.TrackingNumber,
                    Services = order.Services.Select(service => new CustomerOrderServiceDTO
                    {
                        ServiceTypeName = service.ServiceType.Name,
                        Description = service.Description,
                        Price = service.Price,
                        Status = order.Status.ToString(),
                        StartTime = service.StartTime,
                        EndTime = service.EndTime,
                        EstimatedDuration = service.EstimatedDuration,
                        MaterialUsage = service.MaterialId.HasValue ? new MaterialUsageDTO
                        {
                            MaterialName = service.Material.Name,
                            MaterialTypeName = service.Material.MaterialType.Name,
                            Color = service.Material.Color,
                            Quantity = service.MaterialQuantity
                        } : null
                    }).ToList()
                });
                return Ok(orderDtos);
            }
            else
            {
                var orders = await _orderService.GetAllOrdersAsync();
                return Ok(orders);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetOrder(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
                return NotFound();

            if (User.IsInRole(UserRoles.Customer))
            {
                var customerId = int.Parse(User.FindFirst("CustomerId")?.Value ?? "0");
                if (order.CustomerId != customerId)
                    return Forbid();

                var orderDto = new CustomerOrderListDTO
                {
                    Id = order.Id,
                    OrderNumber = order.OrderNumber,
                    CreatedAt = order.CreatedAt,
                    CompletedAt = order.CompletedAt,
                    Status = order.Status.ToString(),
                    FinalPrice = order.FinalPrice,
                    RequiresShipping = order.RequiresShipping,
                    TrackingNumber = order.TrackingNumber,
                    Services = order.Services.Select(service => new CustomerOrderServiceDTO
                    {
                        ServiceTypeName = service.ServiceType.Name,
                        Description = service.Description,
                        Price = service.Price,
                        Status = order.Status.ToString(),
                        StartTime = service.StartTime,
                        EndTime = service.EndTime,
                        EstimatedDuration = service.EstimatedDuration,
                        MaterialUsage = service.MaterialId.HasValue ? new MaterialUsageDTO
                        {
                            MaterialName = service.Material.Name,
                            MaterialTypeName = service.Material.MaterialType.Name,
                            Color = service.Material.Color,
                            Quantity = service.MaterialQuantity
                        } : null
                    }).ToList()
                };
                return Ok(orderDto);
            }

            return Ok(order);
        }

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder([FromBody] Order order)
        {
            if (User.IsInRole(UserRoles.Customer))
            {
                var customerId = int.Parse(User.FindFirst("CustomerId")?.Value ?? "0");
                order.CustomerId = customerId;
            }

            try
            {
                var createdOrder = await _orderService.CreateOrderAsync(order);
                return CreatedAtAction(nameof(GetOrder), new { id = createdOrder.Id }, createdOrder);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<ActionResult> UpdateOrder(int id, [FromBody] Order order)
        {
            if (id != order.Id)
                return BadRequest();

            try
            {
                await _orderService.UpdateOrderAsync(order);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}/status")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<ActionResult> UpdateOrderStatus(int id, [FromBody] OrderStatus status)
        {
            try
            {
                await _orderService.UpdateOrderStatusAsync(id, status);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("by-status/{status}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrdersByStatus(OrderStatus status)
        {
            var orders = await _orderService.GetOrdersByStatusAsync(status);
            return Ok(orders);
        }

        [HttpGet("{id}/cost")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<ActionResult<decimal>> CalculateOrderCost(int id)
        {
            var cost = await _orderService.CalculateOrderCostAsync(id);
            return Ok(new { cost });
        }
    }
}
