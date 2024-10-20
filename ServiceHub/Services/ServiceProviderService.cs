
using ServiceHub.Models;
using ServiceHub.DTOs;
using ServiceHub.DTOs.ServiceHub.DTOs;

namespace ServiceHub.Services
    {
        public class ServiceProviderService
        {
            public ServiceProviderDTO GetServiceProvider(int providerId)
            {
                // Fetch the service provider from the database
                var provider = new ServiceProvider
                {
                    ProviderId = providerId,
                    Name = "Sample Provider",
                    ServiceType = "Sample Service",
                    Availability = "9 AM - 5 PM",
                    Rating = 4.5f,
                    PricePerHour = 50.0f,
                    Location = "Sample Location"
                };

                return new ServiceProviderDTO
                {
                    ProviderId = provider.ProviderId,
                    Name = provider.Name,
                    ServiceType = provider.ServiceType,
                    Availability = provider.Availability,
                    Rating = provider.Rating,
                    PricePerHour = provider.PricePerHour,
                    Location = provider.Location
                };
            }

            public void UpdateServiceProvider(ServiceProviderDTO providerDto)
            {
                var provider = new ServiceProvider
                {
                    ProviderId = providerDto.ProviderId,
                    Name = providerDto.Name,
                    ServiceType = providerDto.ServiceType,
                    Availability = providerDto.Availability,
                    Rating = providerDto.Rating,
                    PricePerHour = providerDto.PricePerHour,
                    Location = providerDto.Location
                };
                // Update the provider in the database
            }
        }
    }

