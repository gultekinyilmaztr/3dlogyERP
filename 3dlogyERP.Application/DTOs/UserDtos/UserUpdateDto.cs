using System.ComponentModel.DataAnnotations;

namespace _3dlogyERP.Application.Dtos.UserDtos
{
    public class UserUpdateDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Kullanıcı adı zorunludur")]
        public string Username { get; set; }

        [Required(ErrorMessage = "E-posta adresi zorunludur")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz")]
        public string Email { get; set; }

        public string Password { get; set; }  // Optional for updates

        [Required(ErrorMessage = "Rol seçimi zorunludur")]
        public string Role { get; set; }

        public bool IsActive { get; set; }
    }
}
