using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = "Admin")]
    public class AdminDashboardController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILoggingService _loggingService;
		private readonly IMapper _mapper;

		public AdminDashboardController(ApplicationDbContext context, ILoggingService loggingService)
        {
            _context = context;
            _loggingService = loggingService;
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers([FromQuery] string search = "", [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var query = _context.UserProfiles.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(u => u.Name.Contains(search) || u.Email.Contains(search) || u.PhoneNumber.Contains(search));
            }

            var users = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(users);
        }

        [HttpGet("users/{userId}")]
        public async Task<IActionResult> GetUserDetails(int userId)
        {
            var user = await _context.UserProfiles.FindAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost("users/{userId}")]
        public async Task<IActionResult> UpdateUserProfile(int userId, [FromBody] UserProfileDto userProfileDto)
        {
            var user = await _context.UserProfiles.FindAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            user.Name = userProfileDto.Name;
            user.Email = userProfileDto.Email;
            user.PhoneNumber = userProfileDto.PhoneNumber;
            user.AccountSettings = userProfileDto.AccountSettings;

            await _context.SaveChangesAsync();

            await _loggingService.LogAdminAction(User.Identity.Name, $"Updated profile for user {userId}");

            return Ok(user);
        }

        [HttpPost("users/{userId}/deactivate")]
        public async Task<IActionResult> DeactivateUser(int userId)
        {
            var user = await _context.UserProfiles.FindAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            user.IsActive = false;
            await _context.SaveChangesAsync();

            await _loggingService.LogAdminAction(User.Identity.Name, $"Deactivated user {userId}");

            return Ok();
        }

        [HttpGet("services")]
        public async Task<IActionResult> GetAllServices()
        {
            var services = await _context.Services.ToListAsync();
            return Ok(services);
        }

        [HttpPost("services")]
        public async Task<IActionResult> AddService([FromBody] ServiceDto serviceDto)
        {
            var service = new Service
            {
                Name = serviceDto.Name,
                Category = serviceDto.Category,
                Description = serviceDto.Description,
                Price = serviceDto.Price
            };

            _context.Services.Add(service);
            await _context.SaveChangesAsync();

            await _loggingService.LogAdminAction(User.Identity.Name, $"Added service {service.Name}");

            return Ok(service);
        }

        [HttpPut("services/{serviceId}")]
        public async Task<IActionResult> UpdateService(int serviceId, [FromBody] ServiceDto serviceDto)
        {
            var service = await _context.Services.FindAsync(serviceId);
            if (service == null)
            {
                return NotFound();
            }

            service.Name = serviceDto.Name;
            service.Category = serviceDto.Category;
            service.Description = serviceDto.Description;
            service.Price = serviceDto.Price;

            await _context.SaveChangesAsync();

            await _loggingService.LogAdminAction(User.Identity.Name, $"Updated service {service.Name}");

            return Ok(service);
        }

        [HttpDelete("services/{serviceId}")]
        public async Task<IActionResult> DeleteService(int serviceId)
        {
            var service = await _context.Services.FindAsync(serviceId);
            if (service == null)
            {
                return NotFound();
            }

            _context.Services.Remove(service);
            await _context.SaveChangesAsync();

            await _loggingService.LogAdminAction(User.Identity.Name, $"Deleted service {service.Name}");

            return Ok();
        }
    }
}
