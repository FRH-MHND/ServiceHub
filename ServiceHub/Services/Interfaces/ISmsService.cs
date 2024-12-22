using ServiceHub.Services.Implementation;

namespace ServiceHub.Services.Interfaces
{
    public interface ISmsService
    {
        Task<NotificationResponse> SendSmsAsync(string serviceProviderName, string phoneNumber, string messageBody);

    }
}