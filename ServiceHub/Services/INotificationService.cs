using System.Threading.Tasks;
using ServiceHub.DTOs;

namespace ServiceHub.Services
{
    public interface INotificationService
    {
        Task NotifyProvider(NotificationDto notification);
        Task NotifyUser(NotificationDto notification);
    }
}
