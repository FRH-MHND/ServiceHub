namespace ServiceHub.DTOs
{
    public class ServiceHistoryDto
    {
        public int Id { get; set; }
        public string ServiceType { get; set; }
        public DateTime Date { get; set; }
        public string ProviderName { get; set; }
        public double Rating { get; set; }
    }
}
