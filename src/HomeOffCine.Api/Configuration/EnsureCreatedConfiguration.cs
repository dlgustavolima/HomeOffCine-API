using HomeOffCine.Api.Data;
using HomeOffCine.Infra.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace HomeOffCine.Api.Configuration;

public static class EnsureCreatedConfiguration
{
    public static WebApplication UseEnsureCreatedConfig(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var dbIdentity = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var dbHomeOffCine = scope.ServiceProvider.GetRequiredService<HomeOffCineDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

            if (dbIdentity.Database.EnsureCreated())
            {
                var user = new IdentityUser
                {
                    UserName = "teste@teste.com.br",
                    NormalizedUserName = "teste@teste.com.br",
                    Email = "teste@teste.com.br",
                    NormalizedEmail = "teste@teste.com.br",
                    EmailConfirmed = true,
                    PhoneNumber = null,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnd = null,
                    LockoutEnabled = true,
                    AccessFailedCount = 0
                };

                userManager.AddPasswordAsync(user, "Teste@123").Wait();
                userManager.CreateAsync(user).Wait();

                user = dbIdentity.Users.FirstOrDefault();

                if (user != null)
                {
                    dbIdentity.UserClaims.Add(new Microsoft.AspNetCore.Identity.IdentityUserClaim<string>
                    {
                        UserId = user.Id,
                        ClaimType = "Filme",
                        ClaimValue = "Adm"
                    });
                }

                dbIdentity.SaveChanges();
                dbIdentity.ChangeTracker.Clear();

                var dataBaseCreator = dbHomeOffCine.GetService<IRelationalDatabaseCreator>();
                dataBaseCreator.CreateTables();
                dbHomeOffCine.ChangeTracker.Clear();
            }
        }

        return app;
    }
}