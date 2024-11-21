using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceHub.Data;
using ServiceHub.DTOs;

namespace ServiceHub.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ServiceController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetServices([FromQuery] string category, [FromQuery] string searchTerm, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var query = _context.Services.AsQueryable();

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(s => s.Category == category);
            }

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(s => s.Name.Contains(searchTerm) || s.Description.Contains(searchTerm));
            }

            var totalItems = await query.CountAsync();
            var services = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(s => new ServiceDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    Category = s.Category,
                    Price = s.Price,
                    AverageRating = s.AverageRating
                })
                .ToListAsync();

            return Ok(new { TotalItems = totalItems, Services = services });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetService(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }

            var serviceDto = new ServiceDto
            {
                Id = service.Id,
                Name = service.Name,
                Description = service.Description,
                Category = service.Category,
                Price = service.Price,
                AverageRating = service.AverageRating
            };

            return Ok(serviceDto);
        }
    }
}
