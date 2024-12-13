using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Store.API.Errors;
using Store.API.Helper;
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
            
            builder.Services.AddDependency(builder.Configuration);
            builder.Services.Configure<ApiBehaviorOptions>(options =>
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

                    return new BadRequestObjectResult (response);
                };
            });
              
            var app = builder.Build();

            await app.ConfigureMiddlewareAsync();

            app.Run();
        }
    }
}
