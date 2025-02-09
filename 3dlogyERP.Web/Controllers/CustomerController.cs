using _3dlogyERP.Application.Dtos.CustomerDtos;
using _3dlogyERP.Application.Interfaces;
using _3dlogyERP.Core.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace _3dlogyERP.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;

        public CustomerController(ICustomerService customerService, IMapper mapper)
        {
            _customerService = customerService;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<ActionResult<IEnumerable<Customer>>> GetAllCustomers()
        {
            var customers = await _customerService.GetAllCustomersAsync();
            return Ok(customers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            var customerId = User.FindFirst("CustomerId")?.Value;
            if (User.IsInRole(UserRoles.Customer) && customerId != id.ToString())
                return Forbid();

            var customer = await _customerService.GetCustomerByIdAsync(id);
            if (customer == null)
                return NotFound();

            return Ok(customer);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCustomer(int id, [FromBody] Customer customer)
        {
            var customerId = User.FindFirst("CustomerId")?.Value;
            if (User.IsInRole(UserRoles.Customer) && customerId != id.ToString())
                return Forbid();

            if (id != customer.Id)
                return BadRequest();

            try
            {

                var customerDto = _mapper.Map<CustomerUpdateDto>(customer);
                await _customerService.UpdateCustomerAsync(id, customerDto);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteCustomer(int id)
        {
            try
            {
                await _customerService.DeleteCustomerAsync(id);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("verify-phone")]
        public async Task<ActionResult> VerifyPhone([FromBody] string verificationCode)
        {
            var customerId = int.Parse(User.FindFirst("CustomerId")?.Value ?? "0");
            if (customerId == 0)
                return BadRequest();

            try
            {
                var result = await _customerService.VerifyPhoneAsync(customerId, verificationCode);
                return Ok(new { verified = result });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
