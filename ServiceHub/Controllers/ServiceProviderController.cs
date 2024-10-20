using Microsoft.AspNetCore.Mvc;
using ServiceHub.Services;
using ServiceHub.DTOs;
using ServiceHub.DTOs.ServiceHub.DTOs;

namespace ServiceHub.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceProviderController : ControllerBase
    {
        private readonly ServiceProviderService _providerService;

        public ServiceProviderController(ServiceProviderService providerService)
        {
            _providerService = providerService;
        }

        [HttpGet("{id}")]
        public IActionResult GetServiceProvider(int id)
        {
            var provider = _providerService.GetServiceProvider(id);
            return Ok(provider);
        }

        [HttpPut("update")]
        public IActionResult UpdateServiceProvider(ServiceProviderDTO providerDto)
        {
            _providerService.UpdateServiceProvider(providerDto);
            return NoContent();
        }
    }
}
