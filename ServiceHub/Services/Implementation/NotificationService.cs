using ServiceHub.DTOs;
using ServiceHub.Services.Interfaces;

public class NotificationService : INotificationService
{
    private readonly IEmailService _emailService;

    public NotificationService(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task NotifyUser(NotificationDto notification)
    {
        var subject = "User Notification";
        var body = notification.Message;
        await _emailService.SendEmailAsync("user@example.com", subject, body);
    }

    public async Task NotifyProvider(NotificationDto notification)
    {
        var subject = "Provider Notification";
        var body = notification.Message;
        await _emailService.SendEmailAsync("provider@example.com", subject, body);
    }

    public async Task NotifyAdmin(NotificationDto notification)
    {
        var subject = "Admin Notification";
        var body = notification.Message;
        await _emailService.SendEmailAsync("admin@example.com", subject, body);
    }

    public async Task SendTestNotification(int userId, string message)
    {
        var notification = new NotificationDto
        {
            RequestId = 0, 
            Message = message,
            IsRead = false
        };

        await NotifyUser(notification);
    }
}
