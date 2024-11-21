using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceHub.Data;
using ServiceHub.DTOs;
using ServiceHub.Models;
using ServiceHub.Services;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceHub.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProviderDashboardController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly INotificationService _notificationService;

        public ProviderDashboardController(ApplicationDbContext context, INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }

        [HttpGet("bookings")]
        public async Task<IActionResult> GetBookings(int providerId)
        {
            var bookings = await _context.Bookings
                .Where(b => b.ServiceProviderId == providerId)
                .ToListAsync();

            var categorizedBookings = bookings.GroupBy(b => b.Status)
                .ToDictionary(g => g.Key, g => g.ToList());

            return Ok(categorizedBookings);
        }

        [HttpPost("status")]
        public async Task<IActionResult> UpdateProviderStatus(int providerId, [FromBody] string status)
        {
            var provider = await _context.ServiceProviders.FindAsync(providerId);
            if (provider == null)
            {
                return NotFound();
            }

            provider.AvailabilityStatus = status;
            await _context.SaveChangesAsync();

            return Ok(provider);
        }

        [HttpPost("accept")]
        public async Task<IActionResult> AcceptBooking([FromBody] BookingStatusDto bookingStatusDto)
        {
            var booking = await _context.Bookings.FindAsync(bookingStatusDto.BookingId);
            if (booking == null)
            {
                return NotFound();
            }

            booking.Status = "Confirmed";
            await _context.SaveChangesAsync();

            await _notificationService.NotifyUser(new NotificationDto
            {
                RequestId = booking.Id,
                Message = "Your booking has been accepted."
            });

            return Ok(booking);
        }

        [HttpPost("decline")]
        public async Task<IActionResult> DeclineBooking([FromBody] BookingStatusDto bookingStatusDto)
        {
            var booking = await _context.Bookings.FindAsync(bookingStatusDto.BookingId);
            if (booking == null)
            {
                return NotFound();
            }

            booking.Status = "Declined";
            await _context.SaveChangesAsync();

            await _notificationService.NotifyUser(new NotificationDto
            {
                RequestId = booking.Id,
                Message = "Your booking has been declined."
            });

            return Ok(booking);
        }
    }
}
