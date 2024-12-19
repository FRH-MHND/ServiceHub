namespace ServiceHub.Models
{
    public class UserProfile
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string AccountSettings { get; set; }
        public bool IsActive { get; set; }
        public bool ReceiveBookingConfirmations { get; set; }
        public bool ReceivePromotions { get; set; }
        public bool ReceiveReminders { get; set; }
        public int UserId { get; internal set; }
    }
}
