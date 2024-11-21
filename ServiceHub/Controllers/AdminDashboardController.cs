using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = "Admin")]
    public class AdminDashboardController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILoggingService _loggingService;

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
    }
}
