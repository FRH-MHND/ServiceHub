namespace ServiceHub.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public int ServiceProviderId { get; set; }
        public int UserId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string IssueDescription { get; set; }
        public string UrgencyLevel { get; set; }
        public string Status { get; set; } = "Pending";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Service Service { get; set; }
        public ServiceProvider ServiceProvider { get; set; }
        public int ? Rating { get; set; }
        public string Feedback { get; set; }
        public DateTime? RescheduledDate { get; set; }
    }
}
