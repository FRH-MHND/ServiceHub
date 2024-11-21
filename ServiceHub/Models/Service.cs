using System;
using System.Collections.Generic;

namespace ServiceHub.Models
{
    public class Service
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public double AverageRating { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
