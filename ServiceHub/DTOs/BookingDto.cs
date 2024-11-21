namespace ServiceHub.DTOs
{
    public class BookingDto
    {
        public int ServiceId { get; set; }
        public int ServiceProviderId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string IssueDescription { get; set; }
        public string UrgencyLevel { get; set; }
    }
}
