using HomeOffCine.Api.Extensions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Net;

namespace HomeOffCine.Api.Configuration;

public static class HealthChecksConfiguration
{
    public static IServiceCollection AddHealthChecksConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks()
            .AddCheck("Movies", new SqlServerHealthCheck(configuration.GetConnectionString("DefaultConnection")))
            .AddSqlServer(configuration.GetConnectionString("DefaultConnection"), healthQuery: "Select 1", name: "HomeOffCine Database", failureStatus: HealthStatus.Unhealthy, tags: new[] { "Feedback", "Database" });

        services.AddHealthChecksUI(options => 
        {
            options.SetEvaluationTimeInSeconds(10);
            options.MaximumHistoryEntriesPerEndpoint(60);
            options.SetApiMaxActiveRequests(1);
            options.AddHealthCheckEndpoint("HC-API", "/api/hc");

        }).AddSqlServerStorage(configuration.GetConnectionString("DefaultConnection"));

        return services;
    }
}
