using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceHub.Data;
using ServiceHub.DTOs;
using ServiceHub.Models;
using ServiceHub.Services;
using System.Threading.Tasks;

namespace ServiceHub.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly INotificationService _notificationService;

        public BookingController(ApplicationDbContext context, INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }

        [HttpPost]
        public async Task<IActionResult> BookService(BookingDto bookingDto)
        {
            var serviceProvider = await _context.ServiceProviders.FindAsync(bookingDto.ServiceProviderId);
            if (serviceProvider == null)
            {
                return NotFound("Service provider not found.");
            }

            // Verify service availability
            var isAvailable = await _context.Bookings
                .AnyAsync(b => b.ServiceProviderId == bookingDto.ServiceProviderId && b.AppointmentDate == bookingDto.AppointmentDate);
            if (isAvailable)
            {
                return BadRequest("The service provider is not available at the selected date and time.");
            }

            var booking = new Booking
            {
                ServiceId = bookingDto.ServiceId,
                ServiceProviderId = bookingDto.ServiceProviderId,
                UserId = 1, // Replace with actual user ID from authentication context
                AppointmentDate = bookingDto.AppointmentDate,
                IssueDescription = bookingDto.IssueDescription,
                UrgencyLevel = bookingDto.UrgencyLevel
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            // Send confirmation notifications
            await _notificationService.NotifyUser(new NotificationDto
            {
                RequestId = booking.Id,
                Message = "Your booking has been confirmed."
            });

            await _notificationService.NotifyProvider(new NotificationDto
            {
                RequestId = booking.Id,
                Message = "You have a new booking."
            });

            return Ok(booking);
        }
    }
}
