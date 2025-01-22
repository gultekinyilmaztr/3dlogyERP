using _3dlogyERP.Application.DTOs;
using _3dlogyERP.Core.Entities;

namespace _3dlogyERP.Application.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDTO> Login(LoginRequestDTO request);
        Task<RegisterResponseDTO> Register(RegisterRequestDTO request);
        Task<User> GetUserById(int id);
        Task<bool> ChangePassword(int userId, ChangePasswordDTO request);
    }
}
