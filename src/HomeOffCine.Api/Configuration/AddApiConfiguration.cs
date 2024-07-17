using Asp.Versioning;

namespace HomeOffCine.Api.Configuration;

public static class AddApiConfiguration
{
    public static IServiceCollection AddApiConfig(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();

        services.AddApiVersioning(options =>
        {
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.ReportApiVersions = true;
        }).AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

        //services.AddCors(options =>
        //{
        //    options.AddPolicy("Development",
        //        builder =>
        //            builder
        //            .AllowAnyOrigin()
        //            .AllowAnyMethod()
        //            .AllowAnyHeader());

        //    options.AddPolicy("Production",
        //        builder =>
        //            builder
        //                .WithMethods("GET")
        //                .WithOrigins("http://homeoffcine.com.br")
        //                .SetIsOriginAllowedToAllowWildcardSubdomains()
        //                //.WithHeaders(HeaderNames.ContentType, "x-custom-header")
        //                .AllowAnyHeader());
        //});

        return services;
    }
}
