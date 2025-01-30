using _3dlogyERP.Application.Dtos.CustomerDtos;

namespace _3dlogyERP.Application.Interfaces
{
    public interface ICustomerService
    {
        Task<CustomerListDto> GetCustomerByIdAsync(int id);
        Task<IEnumerable<CustomerListDto>> GetAllCustomersAsync();
        Task<CustomerListDto> CreateCustomerAsync(CustomerCreateDto customerDto);
        Task<CustomerListDto> UpdateCustomerAsync(int id, CustomerUpdateDto customerDto);
        Task<bool> DeleteCustomerAsync(int id);
        Task<bool> VerifyPhoneAsync(int customerId, string verificationCode);
        Task<CustomerListDto> GetCustomerByEmailAsync(string email);
        Task<CustomerListDto> GetCustomerByPhoneAsync(string phone);
        Task<bool> ExistsByIdAsync(int id);
    }
}
