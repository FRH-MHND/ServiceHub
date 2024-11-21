using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceHub.Data;
using ServiceHub.DTOs;

namespace ServiceHub.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserDashboardController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserDashboardController(ApplicationDbContext context)
        {
            _context = context;
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

            return Ok(userProfile);
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
    }
}
