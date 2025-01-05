using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceHub.Data;
using ServiceHub.DTOs;
using ServiceHub.Services.Interfaces;
using System.Threading.Tasks;
using System.Linq;
using ServiceHub.Models;
using AutoMapper;

namespace ServiceHub.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserDashboardController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly INotificationService _notificationService;
		private readonly IMapper _mapper;

		public UserDashboardController(ApplicationDbContext context, INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetUserProfile(int userId)
        {
            var userProfile = await _context.UserProfiles.FindAsync(userId);
            if (userProfile == null)
            {
                return NotFound();
            }

            var userProfileDto = new UserProfileDto
            {
                Name = userProfile.Name,
                Email = userProfile.Email,
                PhoneNumber = userProfile.PhoneNumber,
                AccountSettings = userProfile.AccountSettings
            };

            return Ok(userProfileDto);
        }

        [HttpPost("profile")]
        public async Task<IActionResult> UpdateUserProfile(int userId, [FromBody] UserProfileDto userProfileDto)
        {
            var userProfile = await _context.UserProfiles.FindAsync(userId);
            if (userProfile == null)
            {
                return NotFound();
            }

            userProfile.Name = userProfileDto.Name;
            userProfile.Email = userProfileDto.Email;
            userProfile.PhoneNumber = userProfileDto.PhoneNumber;
            userProfile.AccountSettings = userProfileDto.AccountSettings;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("appointments")]
        public async Task<IActionResult> GetUpcomingAppointments(int userId)
        {
            var appointments = await _context.Bookings
                .Where(b => b.UserId == userId && b.Status != "Completed")
                .Select(b => new AppointmentDto
                {
                    Id = b.Id,
                    ServiceProviderName = b.ServiceProvider.Name,
                    AppointmentDate = b.AppointmentDate,
                    Status = b.Status
                })
                .ToListAsync();

            return Ok(appointments);
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetServiceHistory(int userId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var bookings = await _context.Bookings
                .Include(b => b.ServiceProvider)
                .Include(b => b.Service)
                .Where(b => b.UserId == userId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var history = bookings.Select(b => new
            {
                ServiceProviderName = b.ServiceProvider.Name,
                ServiceName = b.Service.Name,
                Rating = b.Rating.HasValue ? (double)b.Rating.Value : 0.0
            });

            return Ok(history);
        }

        [HttpGet("notifications")]
        public async Task<IActionResult> GetNotifications(int userId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var notifications = await _context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(n => new NotificationDto
                {
                    RequestId = n.Id,
                    Message = n.Message,
                    IsRead = n.IsRead
                })
                .ToListAsync();

            return Ok(notifications);
        }

        [HttpPost("notifications/mark-read")]
        public async Task<IActionResult> MarkNotificationAsRead(int notificationId)
        {
            var notification = await _context.Notifications.FindAsync(notificationId);
            if (notification == null)
            {
                return NotFound();
            }

            notification.IsRead = true;
            await _context.SaveChangesAsync();

            return Ok(notification);
        }

        [HttpGet("support")]
        public IActionResult GetSupportInfo()
        {
            var supportInfo = new
            {
                FAQs = new[]
                {
                    "How to book a service?",
                    "How to cancel a booking?",
                    "How to contact customer support?"
                },
                Contact = "support@servicehub.com"
            };

            return Ok(supportInfo);
        }

        [HttpGet("feedback/{providerId}")]
        public async Task<IActionResult> GetProviderFeedback(int providerId)
        {
            var feedbacks = await _context.Bookings
                .Where(b => b.ServiceProviderId == providerId && b.Rating.HasValue)
                .Select(b => new
                {
                    b.Rating,
                    b.Feedback
                })
                .ToListAsync();

            return Ok(feedbacks);
        }

        [HttpPost("reschedule")]
        public async Task<IActionResult> RescheduleBooking([FromBody] RescheduleBookingDto rescheduleBookingDto)
        {
            var booking = await _context.Bookings.FindAsync(rescheduleBookingDto.BookingId);
            if (booking == null)
            {
                return NotFound("Booking not found.");
            }

            // Check availability of the new time slot
            var isAvailable = await _context.Bookings
                .AnyAsync(b => b.ServiceProviderId == booking.ServiceProviderId && b.AppointmentDate == rescheduleBookingDto.NewAppointmentDate);
            if (isAvailable)
            {
                return BadRequest("The service provider is not available at the selected date and time.");
            }

            // Update booking details
            booking.RescheduledDate = rescheduleBookingDto.NewAppointmentDate;
            booking.AppointmentDate = rescheduleBookingDto.NewAppointmentDate;
            await _context.SaveChangesAsync();

            // Notify user and provider
            await _notificationService.NotifyUser(new NotificationDto
            {
                RequestId = booking.Id,
                Message = "Your booking has been rescheduled."
            });

            await _notificationService.NotifyProvider(new NotificationDto
            {
                RequestId = booking.Id,
                Message = "A booking has been rescheduled."
            });

            LogReschedulingAction(booking); // Call the logging method

            return Ok(booking);
        }

        [HttpGet("available-slots")]
        public async Task<IActionResult> GetAvailableTimeSlots(int providerId, DateTime date)
        {
            var bookedSlots = await _context.Bookings
                .Where(b => b.ServiceProviderId == providerId && b.AppointmentDate.Date == date.Date)
                .Select(b => b.AppointmentDate)
                .ToListAsync();

            // Assuming service provider works from 9 AM to 5 PM with 1-hour slots
            var availableSlots = Enumerable.Range(9, 8)
                .Select(hour => date.Date.AddHours(hour))
                .Where(slot => !bookedSlots.Contains(slot))
                .ToList();

            return Ok(availableSlots);
        }

        // Add the missing LogReschedulingAction method
        private void LogReschedulingAction(Booking booking)
        {
            // Implement the logging logic here
            // For example, log to a file, database, or any logging service
        }
    }
}
