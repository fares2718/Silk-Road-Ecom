using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SilkRoad.Core;
using SilkRoad.Core.Services;

namespace SilkRoad.Infrastructure;

public static class InfrastructureRegisteration
{
    public static IServiceCollection InfrastructureConfiguration(this IServiceCollection services 
    , IConfiguration configuration)
    {
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddSingleton<IImageManagementService, ImageManagementService>();

        services.AddDbContext<AppDbContext>(options =>{
            options.UseSqlServer(configuration.GetConnectionString("SilkRoadCon"));
        });
        return services;
    }
}
