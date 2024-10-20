using Microsoft.AspNetCore.Mvc;
using ServiceHub.Services;
using ServiceHub.DTOs;
using ServiceHub.DTOs.ServiceHub.DTOs;

namespace ServiceHub.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public IActionResult Register(UserDTO UserDto)
        {
            var user = _userService.RegisterUser(UserDto.Email, UserDto.PhoneNumber, UserDto.Password);
            return Ok(user);
        }

        [HttpPut("update")]
        public IActionResult UpdateProfile(UserDTO userDto)
        {
            _userService.UpdateUserProfile(userDto);
            return NoContent();
        }
    }
}
