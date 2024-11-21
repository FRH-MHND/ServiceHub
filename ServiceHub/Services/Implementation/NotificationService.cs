using System.Threading.Tasks;
using ServiceHub.DTOs;
using ServiceHub.Services.Interfaces;

namespace ServiceHub.Services.Implementation
{
    public class NotificationService : INotificationService
    {
        public Task NotifyProvider(NotificationDto notification)
        {
            // Implement logic to notify provider (e.g., via email, SMS, or push notification)
            return Task.CompletedTask;
        }

        public Task NotifyUser(NotificationDto notification)
        {
            // Implement logic to notify user (e.g., via email, SMS, or push notification)
            return Task.CompletedTask;
        }
    }
}
