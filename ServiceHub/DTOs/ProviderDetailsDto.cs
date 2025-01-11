using System.ComponentModel.DataAnnotations;

namespace ServiceHub.DTOs
{
	public class ProviderDetailsDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string PhoneNumber { get; set; }
		public string ServiceCategory { get; set; }
		public string Status { get; set; }
		public string AvailabilityStatus { get; set; }
	}
}