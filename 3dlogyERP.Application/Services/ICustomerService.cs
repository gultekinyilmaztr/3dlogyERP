using _3dlogyERP.Core.Entities;

namespace _3dlogyERP.Application.Services
{
    public interface ICustomerService
    {
        Task<Customer> GetCustomerByIdAsync(int id);
        Task<IEnumerable<Customer>> GetAllCustomersAsync();
        Task<Customer> CreateCustomerAsync(Customer customer);
        Task UpdateCustomerAsync(Customer customer);
        Task DeleteCustomerAsync(int id);
        Task<bool> VerifyPhoneAsync(int customerId, string verificationCode);
        Task<Customer> GetCustomerByEmailAsync(string email);
        Task<Customer> GetCustomerByPhoneAsync(string phone);
    }
}
