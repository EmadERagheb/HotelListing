﻿using HotelListing.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace HotelListing.Data
{
    public class HotelListingDbcontext : IdentityDbContext<APIUser>
    {

        TimeZoneInfo TimeZoneInfo { get; set; } = TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time");
        public HotelListingDbcontext(DbContextOptions<HotelListingDbcontext> options) : base(options)
        {

        }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Country> Countries { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Very Important
            //When we use IdentityDbcontexty We Must Include The Base OnmodelCreation
            base.OnModelCreating(modelBuilder);

      
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<string>().HaveMaxLength(200);
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {

            DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo);
            var entries = ChangeTracker.Entries<BaseDomainModel>().Where(q => q.State == EntityState.Modified || q.State == EntityState.Added);
            foreach (var entry in entries)
            {
                entry.Entity.UpdateBy = "Ereen";
                entry.Entity.UpdatedDate = localTime;
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedBy = "Emad";
                    entry.Entity.CreatedDate = localTime;
                }

            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }

}
