using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using _3dlogyERP.Core.Entities;
using _3dlogyERP.Core.Interfaces;

namespace _3dlogyERP.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CustomerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Customer> GetCustomerByIdAsync(int id)
        {
            return await _unitOfWork.Customers.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            return await _unitOfWork.Customers.GetAllAsync();
        }

        public async Task<Customer> CreateCustomerAsync(Customer customer)
        {
            if (await _unitOfWork.Customers.AnyAsync(c => c.Email == customer.Email))
                throw new InvalidOperationException("Email already exists");

            if (await _unitOfWork.Customers.AnyAsync(c => c.Phone == customer.Phone))
                throw new InvalidOperationException("Phone number already exists");

            customer.CreatedAt = DateTime.UtcNow;
            customer.IsActive = true;
            customer.IsPhoneVerified = false;

            await _unitOfWork.Customers.AddAsync(customer);
            await _unitOfWork.CompleteAsync();

            return customer;
        }

        public async Task UpdateCustomerAsync(Customer customer)
        {
            var existingCustomer = await _unitOfWork.Customers.GetByIdAsync(customer.Id);
            if (existingCustomer == null)
                throw new InvalidOperationException("Customer not found");

            // Check if email is being changed and if new email already exists
            if (existingCustomer.Email != customer.Email && 
                await _unitOfWork.Customers.AnyAsync(c => c.Email == customer.Email))
                throw new InvalidOperationException("Email already exists");

            // Check if phone is being changed and if new phone already exists
            if (existingCustomer.Phone != customer.Phone && 
                await _unitOfWork.Customers.AnyAsync(c => c.Phone == customer.Phone))
                throw new InvalidOperationException("Phone number already exists");

            _unitOfWork.Customers.Update(customer);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteCustomerAsync(int id)
        {
            var customer = await _unitOfWork.Customers.GetByIdAsync(id);
            if (customer == null)
                throw new InvalidOperationException("Customer not found");

            customer.IsActive = false;
            _unitOfWork.Customers.Update(customer);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<bool> VerifyPhoneAsync(int customerId, string verificationCode)
        {
            var customer = await _unitOfWork.Customers.GetByIdAsync(customerId);
            if (customer == null)
                throw new InvalidOperationException("Customer not found");

            // TODO: Implement actual verification logic
            customer.IsPhoneVerified = true;
            _unitOfWork.Customers.Update(customer);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        public async Task<Customer> GetCustomerByEmailAsync(string email)
        {
            var customers = await _unitOfWork.Customers.FindAsync(c => c.Email == email);
            return customers.FirstOrDefault();
        }

        public async Task<Customer> GetCustomerByPhoneAsync(string phone)
        {
            var customers = await _unitOfWork.Customers.FindAsync(c => c.Phone == phone);
            return customers.FirstOrDefault();
        }
    }
}
