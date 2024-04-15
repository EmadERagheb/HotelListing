using HotelListing.Data;
using HotelListing.Domain;
using HotelListing.WebAPI.Configurations;
using HotelListing.WebAPI.Contracts;
using HotelListing.WebAPI.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
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

            builder.Services.AddControllers().AddOData(option => option.Select().Filter().OrderBy());
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            #region Swagger Configurations
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Hotel Listing API", Version = "v1" });
                options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      Example: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = JwtBearerDefaults.AuthenticationScheme
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme
                            },
                            Scheme = "0auth2",
                            Name = JwtBearerDefaults.AuthenticationScheme,
                            In = ParameterLocation.Header
                        },
                        new List<string>() 
                    } 
                });
            });
            #endregion

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
            #region API Versioning  Not Work right

            //builder.Services.AddApiVersioning(setupAction =>
            //{
            //    setupAction.AssumeDefaultVersionWhenUnspecified = true;
            //    setupAction.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);
            //    setupAction.ReportApiVersions = true;
            //    setupAction.ApiVersionReader = ApiVersionReader.Combine(
            //        new QueryStringApiVersionReader("api-version"),
            //        new HeaderApiVersionReader("X-Version"),
            //        new MediaTypeApiVersionReader("ver")
            //        );
            //}).AddApiExplorer(options =>
            //{
            //    options.GroupNameFormat = "'v'VVV";
            //    options.SubstituteApiVersionInUrl = true;
            //});
            #endregion
            #region Add Cashing
            builder.Services.AddResponseCaching(options =>
            {
                options.MaximumBodySize = 1024;
                options.UseCaseSensitivePaths = true;
            });
            #endregion
            builder.Services.AddHealthChecks();
            var app = builder.Build();


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.MapHealthChecks("/healthCheck");
            app.UseSerilogRequestLogging();

            app.UseHttpsRedirection();
            app.UseCors("AllowAll");

            app.UseResponseCaching();
            app.Use(async (context, next) =>
            {
                context.Response.GetTypedHeaders().CacheControl =
                new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
                {
                    Public = true,
                    MaxAge = TimeSpan.FromSeconds(10),

                };
                context.Response.Headers[HeaderNames.Vary] = new string[] { "Accept-Encoding" };
                await next();
            });

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
