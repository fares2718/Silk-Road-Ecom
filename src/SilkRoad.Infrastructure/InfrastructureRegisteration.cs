using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using SilkRoad.Core;
using SilkRoad.Core.Services;
using StackExchange.Redis;

namespace SilkRoad.Infrastructure;

public static class InfrastructureRegisteration
{
    public static IServiceCollection InfrastructureConfiguration(this IServiceCollection services
    , IConfiguration configuration)
    {
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddSingleton<IImageManagementService, ImageManagementService>();
        services.AddSingleton<IFileProvider>
        (new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));

        services.AddSingleton<IConnectionMultiplexer>(i =>
        {
            var config = ConfigurationOptions.Parse(configuration.GetConnectionString("redis")!);
            return ConnectionMultiplexer.Connect(config);
        });

        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IGenerateToken, GenerateToken>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IPaymentService, PaymentService>();

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("SilkRoadCon"));
        });

        services.AddIdentity<AppUser, IdentityRole>()
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();

        services.AddAuthentication(confOp =>
       {
           confOp.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
           confOp.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
           confOp.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
       })
       //    .AddCookie(confOp =>
       //    {
       //        confOp.Cookie.Name = "token";
       //        confOp.Events.OnRedirectToLogin = context =>
       //        {
       //            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
       //            return Task.CompletedTask;
       //        };
       //    })
       .AddJwtBearer(confOp =>
       {
           confOp.RequireHttpsMetadata = false;
           confOp.SaveToken = true;
           confOp.TokenValidationParameters = new TokenValidationParameters
           {
               ValidateIssuer = true,
               ValidateAudience = true,
               ValidateLifetime = true,
               ValidateIssuerSigningKey = true,
               ValidIssuer = configuration["JWT:Issuer"],
               ValidAudience = configuration["JWT:Audience"],
               IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration["JWT_SECRET_KEY"]!))
           };
           confOp.Events = new JwtBearerEvents
           {

               OnMessageReceived = context =>
               {
                   var token = context.Request.Cookies["token"];
                   if (!string.IsNullOrEmpty(token))
                   {
                       context.Token = token;
                   }
                   return Task.CompletedTask;
               },
               OnChallenge = context =>
        {
            context.HandleResponse(); // Skip the default redirect logic
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";
            return Task.CompletedTask;
        }
           };
       });
        return services;
    }
}
