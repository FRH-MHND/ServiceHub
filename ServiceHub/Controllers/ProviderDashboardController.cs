using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceHub.Data;
using ServiceHub.DTOs;
using ServiceHub.Models;
using ServiceHub.Services.Interfaces;
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
		private readonly IMapper _mapper;

		public ProviderDashboardController(ApplicationDbContext context, INotificationService notificationService, IMapper mapper)
		{
			_context = context;
			_notificationService = notificationService;
			_mapper = mapper;
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
				return NotFound("Service provider not found.");
			}
			if (status != "Available" && status != "Busy" && status != "Offline")
			{
				return BadRequest("Invalid status.");
			}

			provider.AvailabilityStatus = status;
			await _context.SaveChangesAsync();

			return Ok(provider);
		}

		[HttpGet("services/{providerId}")]
		public async Task<IActionResult> GetProviderServices(int providerId)
		{
			var services = await _context.Services
				.Where(s => s.ServiceProviderId == providerId)
				.ToListAsync();

			var provider = await _context.ServiceProviders.FindAsync(providerId);
			if (provider == null)
			{
				return NotFound("Service provider not found.");
			}

			var providerDto = _mapper.Map<ServiceProviderDto>(provider);
			return Ok(new { Provider = providerDto, Services = services });
		}


		[HttpPost("accept")]
        public async Task<IActionResult> AcceptBooking([FromBody] BookingStatusDto bookingStatusDto)
        {
            var booking = await _context.Bookings.FindAsync(bookingStatusDto.BookingId);
            if (booking == null)
            {
                return NotFound("Booking not found.");
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
                return NotFound("Booking not found.");
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

        [HttpPost("feedback")]
        public async Task<IActionResult> SubmitFeedback([FromBody] FeedbackDto feedbackDto)
        {
            var booking = await _context.Bookings.FindAsync(feedbackDto.BookingId);
            if (booking == null)
            {
                return NotFound("Booking not found.");
            }

            booking.Rating = feedbackDto.Rating;
            booking.Feedback = feedbackDto.Feedback;

            var provider = await _context.ServiceProviders.FindAsync(booking.ServiceProviderId);
            if (provider != null)
            {
                provider.AverageRating = await _context.Bookings
                    .Where(b => b.ServiceProviderId == provider.Id && b.Rating.HasValue)
                    .AverageAsync(b => b.Rating.Value);
            }

            await _context.SaveChangesAsync();

            return Ok();
        }
        [HttpPost("services")]
        public async Task<IActionResult> AddService([FromBody] ServiceDto serviceDto)
        {
            var service = new Service
            {
                Name = serviceDto.Name,
                Category = serviceDto.Category,
                Description = serviceDto.Description,
                Price = serviceDto.Price,
                ServiceProviderId = serviceDto.ServiceProviderId
            };

            _context.Services.Add(service);
            await _context.SaveChangesAsync();

            await _notificationService.NotifyAdmin(new NotificationDto
            {
                RequestId = service.Id,
                Message = $"New service added by provider {service.ServiceProviderId} for review."
            });

            return Ok(service);
        }

        [HttpPut("services/{serviceId}")]
        public async Task<IActionResult> UpdateService(int serviceId, [FromBody] ServiceDto serviceDto)
        {
            var service = await _context.Services.FindAsync(serviceId);
            if (service == null)
            {
                return NotFound("Service not found.");
            }

            service.Name = serviceDto.Name;
            service.Category = serviceDto.Category;
            service.Description = serviceDto.Description;
            service.Price = serviceDto.Price;

            await _context.SaveChangesAsync();

            return Ok(service);
        }

        [HttpDelete("services/{serviceId}")]
        public async Task<IActionResult> DeleteService(int serviceId)
        {
            var service = await _context.Services.FindAsync(serviceId);
            if (service == null)
            {
                return NotFound("Service not found.");
            }

            _context.Services.Remove(service);
            await _context.SaveChangesAsync();

            return Ok();
        }
	}
}