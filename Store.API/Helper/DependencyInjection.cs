using Microsoft.EntityFrameworkCore;
using Store.Core.Services.Interfaces;
using Store.Core;
using Store.Repository;
using Store.Repository.Data.Contexts;
using Store.Service.Services.Products;
using Store.Core.Mapping.Products;

namespace Store.API.Helper
{
    public static class DependencyInjection
    {

        public static IServiceCollection AddDependency(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddBuildInService(); 
            services.AddSwaggerService();
            services.AddDbContextService(configuration);
            services.AddUserDefindService();
            services.AddAutoMapperService(configuration);


            return services;
        }

        private static IServiceCollection AddBuildInService(this IServiceCollection services)
        {
            services.AddControllers();

            return services;
        }
        private static IServiceCollection AddSwaggerService(this IServiceCollection services)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
             services.AddEndpointsApiExplorer();
             services.AddSwaggerGen();

            return services;
        }
        private static IServiceCollection AddDbContextService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            return services;
        }
        private static IServiceCollection AddUserDefindService(this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
        private static IServiceCollection AddAutoMapperService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(m => m.AddProfile(new ProductProfile(configuration))); 
            return services;
        }
    } 
}
