using Store.Repository.Data.Contexts;
using Store.Repository.Data;
using Microsoft.EntityFrameworkCore;
using Store.Repository.Identity.Contexts;
using Store.Repository.Identity;
using Microsoft.AspNetCore.Identity;
using Store.Core.Entities.Identity;

namespace Store.API.Helper
{
    public static class ConfigureMiddleware
    {
        public static async Task<WebApplication> ConfigureMiddlewareAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var services = scope.ServiceProvider;

            var context = services.GetRequiredService<StoreDbContext>(); //ASK CLR Create Object StoreDbContext
            var identityContext = services.GetRequiredService<StoreIdentityDbContext>();
            var userManger = services.GetRequiredService<UserManager<AppUser>>();

            var loggerFactory = services.GetRequiredService<ILoggerFactory>();

            try
            {
                await context.Database.MigrateAsync();
                await StoreDbContextSeed.SeedAsync(context);

                await identityContext.Database.MigrateAsync();
                await StoreIdentityDbContextSeed.SeedAppUserAsync(userManger);
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "There Are Problems During Apply Migrations !");
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseStaticFiles();
            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            return app;
        }
    }
}
