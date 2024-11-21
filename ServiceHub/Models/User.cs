namespace ServiceHub.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string PasswordHash { get; set; }
        public string VerificationCode { get; set; }
        public string Status { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsProvider { get; set; } 

    }
}
