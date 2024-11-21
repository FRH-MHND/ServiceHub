using System;

namespace ServiceHub.DTOs
{
    public class ServiceRequestDto
    {
        public int Id { get; set; }
        public string UserLocation { get; set; }
        public string RequestedService { get; set; }
        public DateTime Date { get; set; }
        public DateTime Time { get; set; }
        public int UserId { get; set; } 
    }
}
