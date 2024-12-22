using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceHub.Data;
using ServiceHub.DTOs;
using ServiceHub.Models;
using ServiceHub.Services.Interfaces;
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
                UserId = bookingDto.UserId,
                AppointmentDate = bookingDto.AppointmentDate,
                IssueDescription = bookingDto.IssueDescription,
                UrgencyLevel = bookingDto.UrgencyLevel
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

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


        [HttpPost("cancel")]
        public async Task<IActionResult> CancelBooking([FromBody] CancelBookingDto cancelBookingDto)
        {
            var booking = await _context.Bookings.FindAsync(cancelBookingDto.BookingId);
            if (booking == null)
            {
                return NotFound("Booking not found.");
            }

            booking.Status = "Canceled";
            await _context.SaveChangesAsync();

            var isLateCancellation = booking.AppointmentDate <= DateTime.UtcNow.AddHours(1);
            if (isLateCancellation)
            {
                //LogLateCancellation(booking);

                await _notificationService.NotifyUser(new NotificationDto
                {
                    RequestId = booking.Id,
                    Message = "Your booking has been canceled. A late cancellation fee may apply."
                });
            }

            await _notificationService.NotifyProvider(new NotificationDto
            {
                RequestId = booking.Id,
                Message = "A booking has been canceled."
            });

            var provider = await _context.ServiceProviders.FindAsync(booking.ServiceProviderId);
            if (provider != null)
            {
                provider.AvailabilityStatus = "Available";
                await _context.SaveChangesAsync();
            }

            return Ok(booking);
        }
    }
}
