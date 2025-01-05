using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceHub.Data;
using ServiceHub.DTOs;
using ServiceHub.Models;
using ServiceHub.Services.Interfaces;
using ServiceProvider = ServiceHub.Models.ServiceProvider;

namespace ServiceHub.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceRequestController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly INotificationService _notificationService;

		public ServiceRequestController(ApplicationDbContext context, INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateServiceRequest(ServiceRequestDto serviceRequestDto)
        {
            var serviceRequest = new ServiceRequest
            {
                UserLocation = serviceRequestDto.UserLocation,
                RequestedService = serviceRequestDto.RequestedService,
                Date = serviceRequestDto.Date,
                Time = serviceRequestDto.Time,
                IsAccepted = false,
                IsDeclined = false,
                UserId = serviceRequestDto.UserId 
            };

            _context.ServiceRequests.Add(serviceRequest);
            await _context.SaveChangesAsync();

            var notification = new NotificationDto
            {
                RequestId = serviceRequest.Id,
                Message = $"New service request received: {serviceRequest.RequestedService} at {serviceRequest.UserLocation} on {serviceRequest.Date} at {serviceRequest.Time}."
            };

            await _notificationService.NotifyProvider(notification);

            return Ok(serviceRequest);
        }

        [HttpPost("accept/{id}")]
        public async Task<IActionResult> AcceptServiceRequest(int id)
        {
            var serviceRequest = await _context.ServiceRequests.FindAsync(id);
            if (serviceRequest == null)
            {
                return NotFound();
            }

            serviceRequest.IsAccepted = true;
            await _context.SaveChangesAsync();

            var notification = new NotificationDto
            {
                RequestId = serviceRequest.Id,
                Message = "Your service request has been accepted."
            };

            await _notificationService.NotifyUser(notification);

            return Ok(serviceRequest);
        }

        [HttpPost("decline/{id}")]
        public async Task<IActionResult> DeclineServiceRequest(int id)
        {
            var serviceRequest = await _context.ServiceRequests.FindAsync(id);
            if (serviceRequest == null)
            {
                return NotFound();
            }

            serviceRequest.IsDeclined = true;
            await _context.SaveChangesAsync();

            var notification = new NotificationDto
            {
                RequestId = serviceRequest.Id,
                Message = "Your service request has been declined. We will suggest alternative providers."
            };

            await _notificationService.NotifyUser(notification);

            // Implement logic to suggest alternative providers
            var alternativeProviders = await _context.Users
                .Where(u => u.IsProvider && u.Id != serviceRequest.ProviderId)
                .ToListAsync();

            // Notify alternative providers
            foreach (var provider in alternativeProviders)
            {
                var providerNotification = new NotificationDto
                {
                    RequestId = serviceRequest.Id,
                    Message = $"New alternative service request: {serviceRequest.RequestedService} at {serviceRequest.UserLocation} on {serviceRequest.Date} at {serviceRequest.Time}."
                };
                await _notificationService.NotifyProvider(providerNotification);
            }

            return Ok(serviceRequest);
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterServiceProvider(ServiceProviderDto serviceProviderDto)
        {
            var serviceProvider = new ServiceProvider
            {
                PersonalInformation = serviceProviderDto.PersonalInformation,
                BusinessDetails = serviceProviderDto.BusinessDetails,
                Licenses = serviceProviderDto.Licenses
            };

            _context.ServiceProviders.Add(serviceProvider);
            await _context.SaveChangesAsync();

            return Ok(serviceProvider);
        }
        }
    }
