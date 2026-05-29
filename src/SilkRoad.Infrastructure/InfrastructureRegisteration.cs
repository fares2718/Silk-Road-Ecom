using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
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

        services.AddDbContext<AppDbContext>(options =>{
            options.UseSqlServer(configuration.GetConnectionString("SilkRoadCon"));
        });
        return services;
    }
}
