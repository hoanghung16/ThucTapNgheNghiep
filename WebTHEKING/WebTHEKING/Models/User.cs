using System.ComponentModel.DataAnnotations;

namespace WebTHEKING.Models
{
    public class User
    {
            public int Id { get; set; }

            [Required]
            [StringLength(100)]
            public string Username { get; set; }

            [Required]
            public string PasswordHash { get; set; } // Sẽ lưu mật khẩu đã được mã hóa

            public string FullName { get; set; }
            public string Email { get; set; }
            public string Role { get; set; } // "Admin" hoặc "Customer"
    }
}
