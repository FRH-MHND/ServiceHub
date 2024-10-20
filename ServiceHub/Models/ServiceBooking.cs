namespace ServiceHub.Models
{
    public class ServiceBooking
    {
        public int BookingId { get; set; }
        public string Time { get; set; }
        public string Status { get; set; }

        public void ConfirmBooking()
        {
            // Implement booking confirmation logic here
        }

        public void RescheduleBooking(string newTime)
        {
            // Implement rescheduling logic here
        }

        public void CancelBooking()
        {
            // Implement booking cancellation logic here
        }
    }

}
