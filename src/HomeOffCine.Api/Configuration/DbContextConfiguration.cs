using HomeOffCine.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace HomeOffCine.Api.Configuration;

public static class DbContextConfiguration
{
    public static IServiceCollection ResolveDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<HomeOffCineDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        return services;
    }
}
