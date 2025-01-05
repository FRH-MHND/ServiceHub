using AutoMapper;
using ServiceHub.DTOs;
using ServiceHub.Models;

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
	}
}
