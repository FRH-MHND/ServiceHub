namespace ServiceHub.Models
{
    public class AdminActionLog
    {
        public int Id { get; set; }
        public string AdminName { get; set; }
        public string Action { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
