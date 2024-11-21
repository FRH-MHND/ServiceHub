using System.ComponentModel.DataAnnotations;

namespace ServiceHub.DTOs
{
    public class AdminLoginDto
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
