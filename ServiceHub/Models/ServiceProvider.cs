using System.ComponentModel.DataAnnotations;

namespace ServiceHub.Models
{
    public class ServiceProvider
    {
		public int Id { get; set; }
		public string Name { get; set; }
		public string PhoneNumber { get; set; }
		public string PersonalInformation { get; set; }
        public string PasswordHash { get; set; }
        public string BusinessDetails { get; set; }
		public ServiceCategory ServiceCategory { get; set; }
		public string Licenses { get; set; }
        public string Status { get; set; } = "Pending Verification";
        public string AvailabilityStatus { get; set; } = "Available"; 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public double AverageRating { get; set; }
		public string AboutMe { get; set; }
		public Service? Service { get; set; }


	}
}

