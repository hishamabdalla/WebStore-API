using Microsoft.EntityFrameworkCore;
using Store.Core;
using Store.Repository;
using Store.Repository.Data.Contexts;
using Store.Service.Services.Products;
using Store.Core.Mapping.Products;
using Store.Repository.Identity.Contexts;
using Store.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Store.Core.Services.Contract;
using Store.Service.Services.Tokens;
using Store.Service.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Store.Core.Mapping.Auth;
using Microsoft.AspNetCore.Mvc;
using Store.API.Errors;
using Store.Core.Repositories.Interfaces;
using Store.Repository.Repositories;
using StackExchange.Redis;
using Store.Core.Mapping.Basket;
using Store.Service.Services.Caches;
using Store.Core.Mapping.Orders;
using Store.Service.Services.Orders;
using Store.Service.Services.Payments;
using Role = Store.Core.Entities.Identity.Role;
using Store.Service.Email;
using Store.Core.Entities.Email;
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
            services.AddIdentityService();
            services.AddAuthenticationService(configuration);
            services.ConfigureInvalidModelStateResponseService();
            services.AddRedisService(configuration);
            services.AddEmailConfigurationsService(configuration);

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
            
            services.AddDbContext<StoreIdentityDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("IdentityConnection"));
            });

            return services;
        }
        private static IServiceCollection AddEmailConfigurationsService(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            return services;
        }

        private static IServiceCollection AddUserDefindService(this IServiceCollection services)
        {
           
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IBasketRepository, BasketRepository >();
            services.AddScoped<ICacheService, CacheService >();
            services.AddScoped<IOrderService, OrderService >();
            services.AddScoped<IPaymentService, PaymentService >();
            services.AddScoped<IEmailService, EmailService >();
            services.AddHttpContextAccessor();

            return services;
        }
        private static IServiceCollection AddAutoMapperService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(m => m.AddProfile(new ProductProfile(configuration)));
            services.AddAutoMapper(m => m.AddProfile(new AuthProfile()));
            services.AddAutoMapper(m => m.AddProfile(new BasketProfile()));
            services.AddAutoMapper(m => m.AddProfile(new OrderProfile(configuration)));
            return services;
        }
        private static IServiceCollection ConfigureInvalidModelStateResponseService(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState.Where(P => P.Value.Errors.Count() > 0)
                                             .SelectMany(P => P.Value.Errors)
                                             .Select(E => E.ErrorMessage)
                                             .ToArray();
                    var response = new ApiValidationErrorResponse()
                    {

                        Errors = errors
                    };

                    return new BadRequestObjectResult(response);
                };
            });
            return services;    
        }

        private static IServiceCollection AddIdentityService(this IServiceCollection services)
        {
            
            services.AddIdentity<AppUser,Role>(options =>
            {
                options.SignIn.RequireConfirmedEmail= true;
                // Configure lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15); // Lockout duration
                options.Lockout.MaxFailedAccessAttempts = 5; // Maximum failed attempts before lockout
                options.Lockout.AllowedForNewUsers = true; // Enable lockout for new users
                options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
            })
.AddEntityFrameworkStores<StoreIdentityDbContext>()
.AddDefaultTokenProviders();
            return services;
        }
        private static IServiceCollection AddAuthenticationService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                };
            });

            return services;
        }

        private static IServiceCollection AddRedisService(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddSingleton<IConnectionMultiplexer>((serviceProvider) =>
            {
              

                var connection = configuration.GetConnectionString("Redis");
                return ConnectionMultiplexer.Connect(connection);
            });
            return services;
        }
    } 
}
