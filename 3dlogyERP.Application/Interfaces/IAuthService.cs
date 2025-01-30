using _3dlogyERP.Application.Dtos.UserDtos;
using _3dlogyERP.Application.DTOs.AuthDtos;

namespace _3dlogyERP.Application.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDTO> Login(LoginRequestDTO request);
        Task<RegisterResponseDTO> Register(RegisterRequestDTO request);
        Task<UserListDto> GetUserById(int id);
        Task<bool> ChangePassword(int userId, ChangePasswordDTO request);

    }
}
