using Microsoft.AspNetCore.Mvc;
using ServiceHub.Data;
using ServiceHub.DTOs;
using ServiceHub.Services.Interfaces;

namespace ServiceHub.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ApplicationDbContext _context;
        private readonly INotificationService _notificationService;

        public AdminController(IUserService userService)
        {
            _userService = userService;

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(AdminLoginDto adminLoginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var admin = await _userService.AuthenticateAdmin(adminLoginDto.Email, adminLoginDto.Password);
            if (admin == null)
            {
                return Unauthorized("Invalid credentials.");
            }

            var token = await _userService.GenerateJwtToken(admin);
            return Ok(new { Token = token, Admin = admin });
        }
        [HttpPost("approve/{id}")]
        public async Task<IActionResult> ApproveServiceProvider(int id)
        {
            var serviceProvider = await _context.ServiceProviders.FindAsync(id);
            if (serviceProvider == null)
            {
                return NotFound();
            }

            serviceProvider.Status = "Approved";
            await _context.SaveChangesAsync();

            await _notificationService.NotifyProvider(new NotificationDto
            {
                RequestId = serviceProvider.Id,
                Message = "Your registration has been approved."
            });

            return Ok(serviceProvider);
        }

        [HttpPost("reject/{id}")]
        public async Task<IActionResult> RejectServiceProvider(int id)
        {
            var serviceProvider = await _context.ServiceProviders.FindAsync(id);
            if (serviceProvider == null)
            {
                return NotFound();
            }

            serviceProvider.Status = "Rejected";
            await _context.SaveChangesAsync();

            await _notificationService.NotifyProvider(new NotificationDto
            {
                RequestId = serviceProvider.Id,
                Message = "Your registration has been rejected."
            });

            return Ok(serviceProvider);
        }
    }
}