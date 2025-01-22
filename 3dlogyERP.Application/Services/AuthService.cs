using _3dlogyERP.Application.DTOs;
using _3dlogyERP.Core.Entities;
using _3dlogyERP.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BC = BCrypt.Net.BCrypt;

namespace _3dlogyERP.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public AuthService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            ArgumentNullException.ThrowIfNull(unitOfWork);
            ArgumentNullException.ThrowIfNull(configuration);
            
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO request)
        {
            ArgumentNullException.ThrowIfNull(request);
            ArgumentNullException.ThrowIfNull(request.Email);
            ArgumentNullException.ThrowIfNull(request.Password);

            var user = await GetUserByEmailAsync(request.Email);
            if (user == null || !user.IsActive)
                throw new InvalidOperationException("Invalid credentials");

            if (!await ValidatePasswordAsync(user, request.Password))
                throw new InvalidOperationException("Invalid credentials");

            user.LastLoginAt = DateTime.UtcNow;
            await _unitOfWork.CompleteAsync();

            var token = await GenerateJwtTokenAsync(user);
            var refreshToken = await GenerateRefreshTokenAsync(user);

            return new LoginResponseDTO
            {
                Token = token,
                RefreshToken = refreshToken,
                Username = user.Username,
                Role = user.Role
            };
        }

        public async Task<RegisterResponseDTO> Register(RegisterRequestDTO request)
        {
            ArgumentNullException.ThrowIfNull(request);
            ArgumentNullException.ThrowIfNull(request.Email);
            ArgumentNullException.ThrowIfNull(request.Password);
            ArgumentNullException.ThrowIfNull(request.Username);

            // Check if email already exists
            var existingUser = await GetUserByEmailAsync(request.Email);
            if (existingUser != null)
                throw new InvalidOperationException("Email already registered");

            // Create user account
            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = BC.HashPassword(request.Password),
                Role = UserRoles.Customer,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.CompleteAsync();

            return new RegisterResponseDTO
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role
            };
        }

        public async Task<User> GetUserById(int id)
        {
            return await _unitOfWork.Users.GetByIdAsync(id);
        }

        private async Task<User> GetUserByEmailAsync(string email)
        {
            ArgumentNullException.ThrowIfNull(email);

            var users = await _unitOfWork.Users.FindAsync(u => u.Email == email);
            return users.FirstOrDefault();
        }

        private async Task<bool> ValidatePasswordAsync(User user, string password)
        {
            ArgumentNullException.ThrowIfNull(user);
            ArgumentNullException.ThrowIfNull(password);

            return BC.Verify(password, user.PasswordHash);
        }

        public async Task<bool> ChangePassword(int userId, ChangePasswordDTO request)
        {
            ArgumentNullException.ThrowIfNull(request);
            ArgumentNullException.ThrowIfNull(request.CurrentPassword);
            ArgumentNullException.ThrowIfNull(request.NewPassword);
            ArgumentNullException.ThrowIfNull(request.ConfirmNewPassword);

            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
                throw new InvalidOperationException("User not found");

            if (!await ValidatePasswordAsync(user, request.CurrentPassword))
                throw new InvalidOperationException("Current password is incorrect");

            if (request.NewPassword != request.ConfirmNewPassword)
                throw new InvalidOperationException("New passwords do not match");

            user.PasswordHash = BC.HashPassword(request.NewPassword);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        private async Task<string> GenerateJwtTokenAsync(User user)
        {
            ArgumentNullException.ThrowIfNull(user);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role),
                    new Claim(ClaimTypes.Name, user.Username)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private async Task<string> GenerateRefreshTokenAsync(User user)
        {
            ArgumentNullException.ThrowIfNull(user);

            // Generate a random refresh token
            var randomNumber = new byte[32];
            using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
