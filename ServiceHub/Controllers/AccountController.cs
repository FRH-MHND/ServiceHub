using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ServiceHub.DTOs;
using ServiceHub.Services.Interfaces;

namespace ServiceHub.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
		private readonly IMapper _mapper;

		public AccountController(IUserService userService)
        {
            _userService = userService;

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserDto loginUserDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userService.AuthenticateUser(loginUserDto.Identifier, loginUserDto.Password);
            if (user == null)
            {
                return Unauthorized("Invalid credentials.");
            }

            var token = await _userService.GenerateJwtToken(user);
            return Ok(new { Token = token, User = user });
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto forgotPasswordDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.SendPasswordResetCode(forgotPasswordDto.Identifier);
            if (!result)
            {
                return NotFound("User not found.");
            }

            return Ok("Password reset code sent.");
        }

		[HttpPost("verify-code")]
		public async Task<IActionResult> VerifyCode(string phoneNumber, string code)
		{
			var isValid = await _userService.VerifyCodeAsync(phoneNumber, code);
			if (!isValid)
			{
				return BadRequest("Invalid verification code.");
			}

			return Ok("Verification successful.");
		}

		[HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(string identifier, string newPassword)
        {
            var result = await _userService.ResetPassword(identifier, newPassword);
            if (!result)
            {
                return NotFound("User not found.");
            }

            return Ok("Password reset successful.");
        }
    }
}
