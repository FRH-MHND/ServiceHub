using System.Threading.Tasks;
using ServiceHub.DTOs;

namespace ServiceHub.Services.Interfaces
{
    public interface INotificationService
    {
        Task NotifyProvider(NotificationDto notification);
        Task NotifyUser(NotificationDto notification);
        Task NotifyAdmin(NotificationDto notification);
    }
}
