namespace ServiceHub.DTOs
{
    public class AppointmentDto
    {
        public int Id { get; set; }
        public string ServiceProviderName { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Status { get; set; }
    }
}
