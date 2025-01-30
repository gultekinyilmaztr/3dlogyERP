namespace _3dlogyERP.Application.Dtos.UserDtos
{
    public class UserListDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public string Status => IsActive ? "Aktif" : "Pasif";
    }
}
