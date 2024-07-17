using HomeOffCine.Business.Models;
using HomeOffCine.Infra.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HomeOffCine.Infra.Context;

public class HomeOffCineDbContext : DbContext
{
    public HomeOffCineDbContext(DbContextOptions<HomeOffCineDbContext> options) : base(options)
    {
        
    }

    public DbSet<Movie> Movies { get; set; }
    public DbSet<Rating> Ratings { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseSqlServer(p => p.CommandTimeout(30)
            .EnableRetryOnFailure(3, TimeSpan.FromSeconds(5), null))
            .LogTo(Console.WriteLine, LogLevel.Debug, Microsoft.EntityFrameworkCore.Diagnostics.DbContextLoggerOptions.LocalTime)
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors()
            .AddInterceptors(new InterceptorCommand());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var property in modelBuilder.Model.GetEntityTypes()
                    .SelectMany(e => e.GetProperties()
                    .Where(p => p.ClrType == typeof(string))))
            property.SetColumnType("varchar(100)");

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(HomeOffCineDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

}
