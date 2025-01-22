using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using _3dlogyERP.Application.DTOs;
using _3dlogyERP.Core.Entities;
using _3dlogyERP.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using BC = BCrypt.Net.BCrypt;

namespace _3dlogyERP.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly ICustomerService _customerService;

        public AuthService(IUnitOfWork unitOfWork, IConfiguration configuration, ICustomerService customerService)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _customerService = customerService;
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            var user = await GetUserByEmailAsync(request.Email);
            if (user == null || !user.IsActive)
                throw new InvalidOperationException("Invalid credentials");

            if (!await ValidatePasswordAsync(user, request.Password))
                throw new InvalidOperationException("Invalid credentials");

            user.LastLoginAt = DateTime.UtcNow;
            await _unitOfWork.CompleteAsync();

            var token = await GenerateJwtTokenAsync(user);

            return new AuthResponse
            {
                Token = token,
                Username = user.Username,
                Role = user.Role
            };
        }

        public async Task<AuthResponse> RegisterCustomerAsync(RegisterRequest request)
        {
            // Check if email already exists
            var existingUser = await GetUserByEmailAsync(request.Email);
            if (existingUser != null)
                throw new InvalidOperationException("Email already registered");

            // Create customer first
            var customer = new Customer
            {
                CompanyName = request.CompanyName,
                ContactName = request.ContactName,
                Email = request.Email,
                Phone = request.Phone,
                Address = request.Address,
                TaxNumber = request.TaxNumber,
                TaxOffice = request.TaxOffice,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            await _customerService.CreateCustomerAsync(customer);

            // Create user account
            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = BC.HashPassword(request.Password),
                Role = UserRoles.Customer,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                CustomerId = customer.Id
            };

            await _unitOfWork.CompleteAsync();

            var token = await GenerateJwtTokenAsync(user);

            return new AuthResponse
            {
                Token = token,
                Username = user.Username,
                Role = user.Role
            };
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            var users = await _unitOfWork.Users.FindAsync(u => u.Email == email);
            return users.FirstOrDefault();
        }

        public async Task<bool> ValidatePasswordAsync(User user, string password)
        {
            return BC.Verify(password, user.PasswordHash);
        }

        public async Task<bool> ChangePasswordAsync(int userId, ChangePasswordRequest request)
        {
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

        public async Task<string> GenerateJwtTokenAsync(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role),
                    new Claim("CustomerId", user.CustomerId?.ToString() ?? "")
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<bool> ValidateTokenAsync(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"]);

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
