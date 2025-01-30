using System.ComponentModel.DataAnnotations;

namespace _3dlogyERP.Application.Dtos.UserDtos
{
    public class UserCreateDto
    {
        [Required(ErrorMessage = "Kullanıcı adı zorunludur")]
        public string Username { get; set; }

        [Required(ErrorMessage = "E-posta adresi zorunludur")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre zorunludur")]
        [MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalıdır")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Rol seçimi zorunludur")]
        public string Role { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
