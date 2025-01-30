namespace _3dlogyERP.Application.DTOs.AuthDtos
{
    public class LoginResponseDTO
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
    }
}
