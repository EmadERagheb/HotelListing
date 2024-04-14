using Asp.Versioning;
using HotelListing.Data;
using HotelListing.Domain;
using HotelListing.WebAPI.Configurations;
using HotelListing.WebAPI.Contracts;
using HotelListing.WebAPI.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

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
                .AddTokenProvider<DataProtectorTokenProvider<APIUser>>(builder.Configuration["JWTSettings:Issuer"])
                .AddEntityFrameworkStores<HotelListingDbcontext>()
                .AddDefaultTokenProviders();
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
            builder.Services.AddScoped<IAuthManger, AuthManger>();
            #endregion

            #region JWT
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,

                ClockSkew = TimeSpan.Zero,
                ValidIssuer = builder.Configuration["JWTSettings:Issuer"],
                ValidAudience = builder.Configuration["JWTSettings:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTSettings:Key"]))

            }
            );
            #endregion
            #region API Versioning Not Working
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddApiVersioning(setupAction =>
            {
                setupAction.AssumeDefaultVersionWhenUnspecified = true;
                setupAction.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);
                setupAction.ReportApiVersions = true;
                setupAction.ApiVersionReader = ApiVersionReader.Combine(
                    new QueryStringApiVersionReader("api-version"),
                    new HeaderApiVersionReader("X-Version"),
                    new MediaTypeApiVersionReader("ver")
                    );
            }).AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });
            #endregion
            //builder.Services.AddCascadingValue()







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

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
