using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ServiceHub.Data;
using ServiceHub.DTOs;
using ServiceHub.Models;
using ServiceHub.Services.Interfaces;
using System.Threading.Tasks;

namespace ServiceHub.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ApplicationDbContext _context;
        private readonly INotificationService _notificationService;
        private readonly IMapper _mapper;

        public AdminController(IUserService userService, ApplicationDbContext context, INotificationService notificationService)
        {
            _userService = userService;
            _context = context;
            _notificationService = notificationService;
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

        [HttpPost("register/user")]
        public async Task<IActionResult> RegisterUser(UserRegistrationDto userRegistrationDto)
        {
            Console.WriteLine("seen");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (await _userService.UserExists(userRegistrationDto.PhoneNumber))
            {
                return BadRequest("User with the same email or phone number already exists.");
            }

            var user = new User
            {
                Name = userRegistrationDto.Name,
                PhoneNumber = userRegistrationDto.PhoneNumber,
                PasswordHash = _userService.HashPassword(userRegistrationDto.Password)
            };

            await _userService.CreateUser(user);
            return Ok(user);
            Console.WriteLine("registered");
        }

        [HttpPost("register/provider")]
        public async Task<IActionResult> RegisterProvider(ProviderRegistrationDto providerRegistrationDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (await _userService.UserExists(providerRegistrationDto.PhoneNumber))
            {
                return BadRequest("Provider with the same email or phone number already exists.");
            }

            var provider = new Models.ServiceProvider
            {
                Name = providerRegistrationDto.Name,
                PhoneNumber = providerRegistrationDto.PhoneNumber,
                PasswordHash = _userService.HashPassword(providerRegistrationDto.Password),
                ServiceCategory = providerRegistrationDto.ServiceCategory,
                Status = "Pending"
            };

            _context.ServiceProviders.Add(provider);
            await _context.SaveChangesAsync();

            return Ok(provider);
        }

        [HttpPost("approve/{id}")]
        public async Task<IActionResult> ApproveServiceProvider(int id)
        {
            var serviceProvider = await _context.ServiceProviders.FindAsync(id);
            if (serviceProvider == null)
            {
                return NotFound("Service provider not found.");
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
                return NotFound("Service provider not found.");
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
