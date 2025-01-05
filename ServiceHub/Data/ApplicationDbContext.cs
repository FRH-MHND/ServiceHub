﻿using Microsoft.EntityFrameworkCore;
using ServiceHub.Models;
using ServiceHub.Data;
using ServiceProviderModel = ServiceHub.Models.ServiceProvider;

namespace ServiceHub.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<ServiceRequest> ServiceRequests { get; set; }
        public DbSet<ServiceProviderModel> ServiceProviders { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<FAQ> FAQs { get; set; }
        public DbSet<SupportContact> SupportContacts { get; set; }
        public DbSet<AdminActionLog> AdminActionLogs { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ServiceRequest>()
                .Property(sr => sr.IsExpress)
                .HasDefaultValue(false);

            modelBuilder.Entity<Payment>()
                .Property(p => p.AmountPaid)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Service>()
                .Property(s => s.Price)
                .HasColumnType("decimal(18,2)");
        }
    }
}

