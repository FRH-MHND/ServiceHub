using ServiceHub.Services.Interfaces;
using ServiceHub.DTOs;
using System.Threading.Tasks;
using ServiceHub.Data;
using Microsoft.EntityFrameworkCore;

namespace ServiceHub.Services.Implementation
{
	public class NotificationService : INotificationService
	{
		private readonly ISmsService _smsService;
		private readonly ApplicationDbContext _context;

		public NotificationService(ISmsService smsService, ApplicationDbContext context)
		{
			_smsService = smsService;
			_context = context;
		}

		public async Task NotifyUser(NotificationDto notification)
		{
			var user = await _context.Users.FindAsync(notification.RequestId);
			if (user != null && !string.IsNullOrEmpty(user.PhoneNumber))
			{
				await _smsService.SendSmsAsync("ServiceHub", user.PhoneNumber, notification.Message);
			}
		}

		public async Task NotifyProvider(NotificationDto notification)
		{
			var provider = await _context.ServiceProviders.FindAsync(notification.RequestId);
			if (provider != null)
			{
				var user = await _context.Users.FindAsync(provider.Id);
				if (user != null && !string.IsNullOrEmpty(user.PhoneNumber))
				{
					await _smsService.SendSmsAsync("ServiceHub", user.PhoneNumber, notification.Message);
				}
			}
		}

		public async Task NotifyAdmin(NotificationDto notification)
		{
			var adminUsers = await _context.Users.Where(u => u.IsAdmin).ToListAsync();
			foreach (var admin in adminUsers)
			{
				if (!string.IsNullOrEmpty(admin.PhoneNumber))
				{
					await _smsService.SendSmsAsync("ServiceHub", admin.PhoneNumber, notification.Message);
				}
			}
		}

		public async Task SendTestNotification(int userId, string message)
		{
			var user = await _context.Users.FindAsync(userId);
			if (user != null && !string.IsNullOrEmpty(user.PhoneNumber))
			{
				await _smsService.SendSmsAsync("ServiceHub", user.PhoneNumber, message);
			}
		}
	}
}
