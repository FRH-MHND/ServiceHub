using System;

namespace ServiceHub.Models
{
    public class ServiceRequest
    {
        public int Id { get; set; }
        public string UserLocation { get; set; }
        public string RequestedService { get; set; }
        public DateTime Date { get; set; }
        public DateTime Time { get; set; }
        public bool IsAccepted { get; set; }
        public bool IsDeclined { get; set; }
        public int ProviderId { get; set; }
        public int UserId { get; set; }
        public bool IsExpress { get; set; }
    }
}
