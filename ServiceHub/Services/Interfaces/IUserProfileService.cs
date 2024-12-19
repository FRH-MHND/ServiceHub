using ServiceHub.Data;

namespace ServiceHub.Services.Interfaces
{
    public interface IUserProfileService
    {
        Task UpdateNotificationPreferences(int userId, NotificationPreferencesDto preferences);
    }
}
