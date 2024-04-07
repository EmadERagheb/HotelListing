
using HotelListing.Data;
using HotelListing.Domain;
using HotelListing.WebAPI.Configurations;
using HotelListing.WebAPI.Contracts;
using HotelListing.WebAPI.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace HotelListing.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            #region Build CORS policy
            builder.Services.AddCors(options =>
           {
               options.AddPolicy("AllowAll", setup =>
               setup.AllowAnyHeader()
               .AllowAnyOrigin()
               .AllowAnyMethod());
           });
            #endregion

            #region Build Context And Identity
            builder.Services.AddDbContext<HotelListingDbcontext>(options =>
              {
                  options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), setup =>
                  {
                      setup.CommandTimeout(30);
                      setup.EnableRetryOnFailure(maxRetryCount: 5,
                          maxRetryDelay: TimeSpan.FromSeconds(5),
                           errorNumbersToAdd: null);
                  })
                  .LogTo(Console.WriteLine, LogLevel.Information)
                  .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                  if (!builder.Environment.IsDevelopment())
                  {
                      options.EnableDetailedErrors();
                      options.EnableSensitiveDataLogging();
                  }

              });
            builder.Services.AddIdentityCore<APIUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<HotelListingDbcontext>();
            #endregion

            #region Build Logger
            builder.Host.UseSerilog((ctx, lc) => lc.WriteTo.Console().ReadFrom.Configuration(ctx.Configuration));
            #endregion

            #region Build AutoMapper Service
            builder.Services.AddAutoMapper(typeof(MapperConfig));
            #endregion

            #region IOC
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<ICountriesRepository, CountriesRepository>();
            builder.Services.AddScoped<IHotelRepository, HotelRepository>();
            #endregion

            var app = builder.Build();


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseSerilogRequestLogging();

            app.UseHttpsRedirection();
            app.UseCors("AllowAll");

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
