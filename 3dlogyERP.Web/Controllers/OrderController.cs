using _3dlogyERP.Application.Dtos.OrderDtos;
using _3dlogyERP.Application.Interfaces;
using _3dlogyERP.Core.Entities;
using _3dlogyERP.Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace _3dlogyERP.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrderController> _logger;

        public OrderController(IOrderService orderService, ILogger<OrderController> logger)
        {
            ArgumentNullException.ThrowIfNull(orderService);
            ArgumentNullException.ThrowIfNull(logger);

            _orderService = orderService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderListDto>>> GetAllOrders()
        {
            try
            {
                if (User.IsInRole(UserRoles.Customer))
                {
                    var customerIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                    if (customerIdClaim == null || !int.TryParse(customerIdClaim.Value, out int customerId))
                    {
                        return BadRequest("Invalid customer ID");
                    }
                    var customerOrders = await _orderService.GetCustomerOrdersAsync(customerId);
                    return Ok(customerOrders);
                }

                var orders = await _orderService.GetAllOrdersAsync();
                return Ok(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting orders");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<OrderListDto>> GetOrderById(int id)
        {
            try
            {
                var order = await _orderService.GetOrderByIdAsync(id);
                if (order == null)
                    return NotFound();

                if (User.IsInRole(UserRoles.Customer))
                {
                    var customerIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                    if (customerIdClaim == null || !int.TryParse(customerIdClaim.Value, out int customerId) || order.CustomerId != customerId)
                    {
                        return Forbid();
                    }
                }

                return Ok(order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting order with ID: {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("number/{orderNumber}")]
        public async Task<ActionResult<OrderListDto>> GetOrderByNumber(string orderNumber)
        {
            try
            {
                var order = await _orderService.GetOrderByNumberAsync(orderNumber);
                if (order == null)
                    return NotFound();

                if (User.IsInRole(UserRoles.Customer))
                {
                    var customerIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                    if (customerIdClaim == null || !int.TryParse(customerIdClaim.Value, out int customerId) || order.CustomerId != customerId)
                    {
                        return Forbid();
                    }
                }

                return Ok(order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting order with number: {OrderNumber}", orderNumber);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<OrderListDto>> CreateOrder([FromBody] OrderCreateDto orderDto)
        {
            try
            {
                if (User.IsInRole(UserRoles.Customer))
                {
                    var customerIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                    if (customerIdClaim == null || !int.TryParse(customerIdClaim.Value, out int customerId))
                    {
                        return BadRequest("Invalid customer ID");
                    }
                    orderDto.CustomerId = customerId;
                }

                var order = await _orderService.CreateOrderAsync(orderDto);
                return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating order");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<ActionResult<OrderListDto>> UpdateOrder(int id, [FromBody] OrderUpdateDto orderDto)
        {
            try
            {
                var updatedOrder = await _orderService.UpdateOrderAsync(id, orderDto);
                if (updatedOrder == null)
                    return NotFound();

                return Ok(updatedOrder);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating order with ID: {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id:int}/status")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<ActionResult<OrderListDto>> UpdateOrderStatus(int id, [FromBody] OrderStatus status)
        {
            try
            {
                var updatedOrder = await _orderService.UpdateOrderStatusAsync(id, status);
                if (updatedOrder == null)
                    return NotFound();

                return Ok(updatedOrder);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating status for order with ID: {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("{id:int}/cancel")]
        public async Task<IActionResult> CancelOrder(int id)
        {
            try
            {
                if (User.IsInRole(UserRoles.Customer))
                {
                    var customerIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                    if (customerIdClaim == null || !int.TryParse(customerIdClaim.Value, out int customerId))
                    {
                        return BadRequest("Invalid customer ID");
                    }

                    var order = await _orderService.GetOrderByIdAsync(id);
                    if (order == null)
                        return NotFound();

                    if (order.CustomerId != customerId)
                        return Forbid();
                }

                var result = await _orderService.CancelOrderAsync(id);
                if (!result)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error canceling order with ID: {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("by-status/{status}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<ActionResult<IEnumerable<OrderListDto>>> GetOrdersByStatus(OrderStatus status)
        {
            try
            {
                var orders = await _orderService.GetOrdersByStatusAsync(status);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting orders by status: {Status}", status);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id:int}/total")]
        public async Task<ActionResult<decimal>> CalculateOrderTotal(int id)
        {
            try
            {
                if (User.IsInRole(UserRoles.Customer))
                {
                    var customerIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                    if (customerIdClaim == null || !int.TryParse(customerIdClaim.Value, out int customerId))
                    {
                        return BadRequest("Invalid customer ID");
                    }

                    var order = await _orderService.GetOrderByIdAsync(id);
                    if (order == null)
                        return NotFound();

                    if (order.CustomerId != customerId)
                        return Forbid();
                }

                var total = await _orderService.CalculateOrderTotalAsync(id);
                return Ok(new { total });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating total for order with ID: {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id:int}/cost")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<ActionResult<decimal>> CalculateOrderCost(int id)
        {
            try
            {
                var cost = await _orderService.CalculateOrderCostAsync(id);
                return Ok(new { cost });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating cost for order with ID: {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}