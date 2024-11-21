using System.ComponentModel.DataAnnotations;

namespace ServiceHub.DTOs
{
    public class LoginUserDto
    {
        [Required]
        public string Identifier { get; set; } // Can be email or phone number

        [Required]
        public string Password { get; set; }
    }
}
