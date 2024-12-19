namespace ServiceHub.DTOs
{
    public class PaymentHistoryDto
    {
        public string ServiceType { get; set; }
        public string ProviderName { get; set; }
        public DateTime Date { get; set; }
        public decimal AmountPaid { get; set; }
        public string PaymentMethod { get; set; }
    }

}
