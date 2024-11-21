using System.ComponentModel.DataAnnotations;

namespace ServiceHub.DTOs
{
    public class ForgotPasswordDto
    {
        [Required]
        public string Identifier { get; set; } // Can be email or phone number
    }
}
