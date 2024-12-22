using Microsoft.EntityFrameworkCore;
using ServiceHub.Data;
using ServiceHub.DTOs;

namespace ServiceHub.Services.Implementation
{
    public class PaymentService
    {
        private readonly ApplicationDbContext _context;

        public PaymentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PaymentHistoryDto>> GetPaymentHistoryAsync(int userId, DateTime? startDate, DateTime? endDate, string serviceType)
        {
            var query = _context.Payments.Where(p => p.UserId == userId);

            if (startDate.HasValue)
            {
                query = query.Where(p => p.Date >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(p => p.Date <= endDate.Value);
            }

            if (!string.IsNullOrEmpty(serviceType))
            {
                query = query.Where(p => p.ServiceType == serviceType);
            }

            return await query.Select(p => new PaymentHistoryDto
            {
                ServiceType = p.ServiceType,
                ProviderName = p.ProviderName,
                Date = p.Date,
                AmountPaid = p.AmountPaid,
                PaymentMethod = p.PaymentMethod
            }).ToListAsync();
        }
    }

}
