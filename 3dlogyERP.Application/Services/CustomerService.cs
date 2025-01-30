using _3dlogyERP.Application.Dtos.CustomerDtos;
using _3dlogyERP.Application.Interfaces;
using _3dlogyERP.Core.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace _3dlogyERP.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CustomerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            ArgumentNullException.ThrowIfNull(unitOfWork);
            ArgumentNullException.ThrowIfNull(mapper);

            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CustomerListDto> GetCustomerByIdAsync(int id)
        {
            var customer = await _unitOfWork.Customers
                .Query()
                .Include(c => c.Orders)
                .FirstOrDefaultAsync(c => c.Id == id);

            return _mapper.Map<CustomerListDto>(customer);
        }

        public async Task<IEnumerable<CustomerListDto>> GetAllCustomersAsync()
        {
            var customers = await _unitOfWork.Customers
                .Query()
                .Include(c => c.Orders)
                .ToListAsync();

            return _mapper.Map<IEnumerable<CustomerListDto>>(customers);
        }

        public async Task<CustomerListDto> CreateCustomerAsync(CustomerCreateDto customerDto)
        {
            ArgumentNullException.ThrowIfNull(customerDto);

            if (await _unitOfWork.Customers.AnyAsync(c => c.Email == customerDto.Email))
                throw new InvalidOperationException("Email already exists");

            if (await _unitOfWork.Customers.AnyAsync(c => c.Phone == customerDto.Phone))
                throw new InvalidOperationException("Phone number already exists");

            var customer = _mapper.Map<Customer>(customerDto);
            customer.CreatedAt = DateTime.UtcNow;
            customer.IsPhoneVerified = false;

            await _unitOfWork.Customers.AddAsync(customer);
            await _unitOfWork.CompleteAsync();

            return await GetCustomerByIdAsync(customer.Id);
        }

        public async Task<CustomerListDto> UpdateCustomerAsync(int id, CustomerUpdateDto customerDto)
        {
            ArgumentNullException.ThrowIfNull(customerDto);

            var existingCustomer = await _unitOfWork.Customers.GetByIdAsync(id);
            if (existingCustomer == null)
                throw new InvalidOperationException("Customer not found");

            // Check if email is being changed and if new email already exists
            if (existingCustomer.Email != customerDto.Email &&
                await _unitOfWork.Customers.AnyAsync(c => c.Email == customerDto.Email))
                throw new InvalidOperationException("Email already exists");

            // Check if phone is being changed and if new phone already exists
            if (existingCustomer.Phone != customerDto.Phone &&
                await _unitOfWork.Customers.AnyAsync(c => c.Phone == customerDto.Phone))
                throw new InvalidOperationException("Phone number already exists");

            _mapper.Map(customerDto, existingCustomer);
            await _unitOfWork.CompleteAsync();

            return await GetCustomerByIdAsync(id);
        }

        public async Task<bool> DeleteCustomerAsync(int id)
        {
            var customer = await _unitOfWork.Customers.GetByIdAsync(id);
            if (customer == null)
                return false;

            _unitOfWork.Customers.Remove(customer);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<bool> VerifyPhoneAsync(int customerId, string verificationCode)
        {
            var customer = await _unitOfWork.Customers.GetByIdAsync(customerId);
            if (customer == null)
                return false;

            // TODO: Implement actual verification logic
            customer.IsPhoneVerified = true;
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<CustomerListDto> GetCustomerByEmailAsync(string email)
        {
            var customer = await _unitOfWork.Customers
                .Query()
                .Include(c => c.Orders)
                .FirstOrDefaultAsync(c => c.Email == email);

            return _mapper.Map<CustomerListDto>(customer);
        }

        public async Task<CustomerListDto> GetCustomerByPhoneAsync(string phone)
        {
            var customer = await _unitOfWork.Customers
                .Query()
                .Include(c => c.Orders)
                .FirstOrDefaultAsync(c => c.Phone == phone);

            return _mapper.Map<CustomerListDto>(customer);
        }

        public async Task<bool> ExistsByIdAsync(int id)
        {
            return await _unitOfWork.Customers
                .Query()
                .AnyAsync(c => c.Id == id);
        }
    }
}
