public class Payment
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string ServiceType { get; set; }
    public string ProviderName { get; set; }
    public DateTime Date { get; set; }
    public decimal AmountPaid { get; set; }
    public string PaymentMethod { get; set; }
}
