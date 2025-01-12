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

		public AdminDashboardController(ApplicationDbContext context, ILoggingService loggingService, IMapper mapper)
		{
			_context = context;
			_loggingService = loggingService;
			_mapper = mapper;
		}

		[HttpGet("users/{userId}")]
		public async Task<IActionResult> GetUserDetails(int userId)
		{
			var user = await _context.UserProfiles.FindAsync(userId);
			if (user == null)
			{
				return NotFound();
			}

			var userDto = _mapper.Map<UserProfileDto>(user);
			return Ok(userDto);
		}

		[HttpPost("users/{userId}")]
public async Task<IActionResult> UpdateUserProfile(int userId, [FromBody] UserProfileDto userProfileDto)
{
    var user = await _context.UserProfiles.FindAsync(userId);
    if (user == null)
    {
        return NotFound();
    }

    // Manually map properties, excluding the Id
    user.Name = userProfileDto.Name;
    user.Email = userProfileDto.Email;
    // Map other properties as needed

    await _context.SaveChangesAsync();

    return Ok();
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

			return Ok();
		}

		[HttpGet("services")]
		public async Task<IActionResult> GetAllServices()
		{
			var services = await _context.Services.ToListAsync();
			var serviceDtos = _mapper.Map<List<ServiceDto>>(services);
			return Ok(serviceDtos);
		}

		[HttpPost("services")]
		public async Task<IActionResult> AddService([FromBody] ServiceDto serviceDto)
		{
			var service = _mapper.Map<Service>(serviceDto);
			_context.Services.Add(service);
			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(GetAllServices), new { id = service.Id }, serviceDto);
		}

		[HttpPut("services/{serviceId}")]
	public async Task<IActionResult> UpdateService(int serviceId, [FromBody] ServiceDto serviceDto)
{
    // Retrieve the service from the database
    var service = await _context.Services.FindAsync(serviceId);
    if (service == null)
    {
        return NotFound("Service not found.");
    }

    // Manually map properties that are updatable
    service.Name = serviceDto.Name; // Example property
    service.Description = serviceDto.Description; // Example property
    service.Price = serviceDto.Price; // Example property

    // Save changes to the database
    await _context.SaveChangesAsync();

    return Ok("Service updated successfully.");
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

			return NoContent();
		}
	}
}
