using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ServiceHub.Services.Implementation;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserProfileService _userProfileService;
    private readonly NotificationService _notificationService;
	private readonly IMapper _mapper;

	public UserController(UserProfileService userProfileService, NotificationService notificationService)
    {
        _userProfileService = userProfileService;
        _notificationService = notificationService;
    }

    [HttpPost("preferences")]
    public async Task<IActionResult> UpdateNotificationPreferences([FromBody] NotificationPreferencesDto preferences)
    {
        var userId = GetUserIdFromToken();
        await _userProfileService.UpdateNotificationPreferences(userId, preferences);
        await _notificationService.SendTestNotification(userId, "Your notification preferences have been updated.");
        return Ok();
    }

    private int GetUserIdFromToken()
    {
        var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);
        var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
        var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        return int.Parse(userIdClaim.Value);
    }
}
