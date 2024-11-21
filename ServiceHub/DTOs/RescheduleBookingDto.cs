namespace ServiceHub.DTOs
{
    public class RescheduleBookingDto
    {
        public int BookingId { get; set; }
        public DateTime NewAppointmentDate { get; set; }
    }
}
