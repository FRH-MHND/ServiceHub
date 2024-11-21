namespace ServiceHub.Models
{
    public class ServiceProvider
    {
        public int Id { get; set; }
        public string Name { get; set; } 
        public string PersonalInformation { get; set; }
        public string BusinessDetails { get; set; }
        public string Licenses { get; set; }
        public string Status { get; set; } = "Pending Verification";
        public string AvailabilityStatus { get; set; } = "Available"; 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public double AverageRating { get; set; }
    }
}
