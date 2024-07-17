using Asp.Versioning.ApiExplorer;
using HealthChecks.UI.Client;
using HomeOffCine.Api.Configuration;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
    .AddEnvironmentVariables();

builder.Services.ResolveDbContext(builder.Configuration);

builder.Services.ResolveIdentity(builder.Configuration);

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.ResolveDependencies();

builder.Services.AddApiConfig();

builder.Services.AddSwaggerConfig();

builder.Services.AddHealthChecksConfig(builder.Configuration);

var app = builder.Build();
var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

app.MapControllers();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseSwaggerConfig(apiVersionDescriptionProvider);

app.UseEnsureCreatedConfig();

app.MapHealthChecks("/api/hc", new HealthCheckOptions() { Predicate = _ => true, ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse });
app.MapHealthChecksUI(options => 
{
    options.UIPath = "/api/hc-ui";
    options.ResourcesPath = "/api/hc-ui-resources";

    options.UseRelativeApiPath = false;
    options.UseRelativeResourcesPath = false;
    options.UseRelativeWebhookPath = false;
});

app.Run();
