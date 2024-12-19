using Microsoft.EntityFrameworkCore;
using ServiceHub.Data;
using ServiceHub.Services.Interfaces;

public class UserProfileService : IUserProfileService
{
    private readonly ApplicationDbContext _context;

    public UserProfileService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task UpdateNotificationPreferences(int userId, NotificationPreferencesDto preferences)
    {
        var userProfile = await _context.UserProfiles.FirstOrDefaultAsync(up => up.UserId == userId);
        if (userProfile == null)
        {
            throw new Exception("User profile not found.");
        }

        userProfile.ReceiveBookingConfirmations = preferences.ReceiveBookingConfirmations;
        userProfile.ReceivePromotions = preferences.ReceivePromotions;
        userProfile.ReceiveReminders = preferences.ReceiveReminders;

        await _context.SaveChangesAsync();
    }
}
