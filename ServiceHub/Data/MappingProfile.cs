using AutoMapper;
using ServiceHub.DTOs;
using ServiceHub.Models;
using ServiceProvider = ServiceHub.Models.ServiceProvider;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
		CreateMap<UserProfile, UserProfileDto>().ReverseMap();
		CreateMap<Notification, NotificationDto>().ReverseMap();
		CreateMap<Service, ServiceDto>().ReverseMap();
		CreateMap<User, UserRegistrationDto>().ReverseMap();
		CreateMap<Booking, BookingStatusDto>().ReverseMap();
		CreateMap<Booking, CancelBookingDto>().ReverseMap();
		CreateMap<ServiceProvider, ProviderDto>().ReverseMap();
		CreateMap<User, UserProfileDto>();
		CreateMap<ServiceProvider, ServiceProviderDto>();
		CreateMap<UserProfile, UserProfileDto>().ReverseMap();
		CreateMap<Service, ServiceDto>().ReverseMap();
	}
}
