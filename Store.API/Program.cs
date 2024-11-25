using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Store.Core;
using Store.Core.Mapping.Products;
using Store.Core.Services.Interfaces;
using Store.Repository;
using Store.Repository.Data;
using Store.Repository.Data.Contexts;
using Store.Service.Services.Products;
namespace Store.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            builder.Services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddAutoMapper(m => m.AddProfile(new ProductProfile(builder.Configuration)));
            var app = builder.Build();

            using var scope = app.Services.CreateScope();

            var services = scope.ServiceProvider;

            var context = services.GetRequiredService<StoreDbContext>();
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();

            try
            {
                await context.Database.MigrateAsync();
                await StoreDbContextSeed.SeedAsync(context);

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

            app.Run();
        }
    }
}
