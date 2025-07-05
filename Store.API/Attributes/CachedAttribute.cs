using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using Store.Core.Services.Contract;
using System.Text;

namespace Store.API.Attributes
{
    public class CachedAttribute:Attribute,IAsyncActionFilter
    {
        private readonly int expireTime;

        public CachedAttribute(int expireTime)
        {
            this.expireTime = expireTime;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cacheService=  context.HttpContext.RequestServices.GetRequiredService<ICacheService>();

            var cacheKey = GenerateCacheKeyFormRequest(context.HttpContext.Request);

           var cacheResponse = await cacheService.GetCacheKeyAsync(cacheKey);

            if (!string.IsNullOrEmpty(cacheResponse))
            {
                var contentResult = new ContentResult()
                {
                    Content = cacheResponse,
                    StatusCode = 200,
                    ContentType = "application/json"
                };

                context.Result = contentResult;
                return;
            }
             var executedContext= await next();
            if (executedContext.Result is OkObjectResult response)
            {
                await cacheService.SetCacheKeyAsync(cacheKey,response.Value,TimeSpan.FromDays(expireTime));
            }
        }

        private string GenerateCacheKeyFormRequest(HttpRequest request)
        {
            var cacheKey = new StringBuilder();
            cacheKey.Append($"{request.Path}");

            foreach (var (key,value) in request.Query.OrderBy(x=>x.Key))
            {
                cacheKey.Append($"|{key}-{value}");
            }

            return cacheKey.ToString();
        }
    }
}
