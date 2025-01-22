using System.Threading.Tasks;
using _3dlogyERP.Application.DTOs;
using _3dlogyERP.Core.Entities;

namespace _3dlogyERP.Application.Services
{
    public interface IAuthService
    {
        Task<AuthResponse> LoginAsync(LoginRequest request);
        Task<AuthResponse> RegisterCustomerAsync(RegisterRequest request);
        Task<User> GetUserByEmailAsync(string email);
        Task<bool> ValidatePasswordAsync(User user, string password);
        Task<bool> ChangePasswordAsync(int userId, ChangePasswordRequest request);
        Task<string> GenerateJwtTokenAsync(User user);
        Task<bool> ValidateTokenAsync(string token);
    }
}
