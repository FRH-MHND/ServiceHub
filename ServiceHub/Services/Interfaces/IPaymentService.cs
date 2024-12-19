using ServiceHub.DTOs;

namespace ServiceHub.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<IEnumerable<PaymentHistoryDto>> GetPaymentHistoryAsync(int userId, DateTime? startDate, DateTime? endDate, string serviceType);
    }
}
