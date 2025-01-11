namespace ServiceHub.Models
{
	public class ServiceCategory
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public ICollection<ServiceProvider> ServiceProviders { get; set; }
	}
}
