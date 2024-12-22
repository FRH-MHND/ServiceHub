using ServiceHub.DTOs;

namespace ServiceHub.Services.Interfaces
{
    public interface INotificationService
    {
        Task NotifyUser(NotificationDto notification);
        Task NotifyProvider(NotificationDto notification);
        Task NotifyAdmin(NotificationDto notification);
        Task SendTestNotification(int userId, string message);
    }
}
