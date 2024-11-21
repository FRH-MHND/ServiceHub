using ServiceHub.Data;
using ServiceHub.Models;
using ServiceHub.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace ServiceHub.Services.Implementation
{
    public class LoggingService : ILoggingService
    {
        private readonly ApplicationDbContext _context;

        public LoggingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task LogAdminAction(string adminName, string action)
        {
            var log = new AdminActionLog
            {
                AdminName = adminName,
                Action = action,
                Timestamp = DateTime.UtcNow
            };

            _context.AdminActionLogs.Add(log);
            await _context.SaveChangesAsync();
        }
    }
}
