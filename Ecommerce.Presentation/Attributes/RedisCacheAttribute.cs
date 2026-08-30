using Ecommerce.ServiceAbstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Presentation.Attributes
{
    public class RedisCacheAttribute : ActionFilterAttribute
    {
        private readonly int _expirationDuration;
        public RedisCacheAttribute(int expirationDuration = 5)
        {
            _expirationDuration = expirationDuration;
        }
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // get cahce service from dependency injection DI container
            var CacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();
            // create cache key based on request path and query string parameters
            var cacheKey = CreateCacheKey(context.HttpContext.Request);
            // check if cahced data exist of not
            var cachedValue = CacheService.GetAsync(cacheKey);
            if (cachedValue.Result is not null)
            {
                context.Result = new ContentResult()
                {
                    Content = cachedValue.Result,
                    ContentType = "application/json",
                    StatusCode = StatusCodes.Status200OK
                };
                return;
            }
            //if exist?, return the data wihtout executing the action method
            //if not exist?, execute the action method and cache the data if the result is success (200 Ok)
            var executedContext = await next.Invoke(); // execute the endpoint
            if (executedContext.Result is OkObjectResult result)
            {
                await CacheService.SetAsync(cacheKey, System.Text.Json.JsonSerializer.Serialize(result.Value), TimeSpan.FromMinutes(_expirationDuration)); // cache the data for 30 minutes
            }

        }

        private string CreateCacheKey(HttpRequest request)
        {
            StringBuilder key = new StringBuilder();
            key.Append(request.Path.ToString().ToLower()); // api/products
            foreach (var (keyName, value) in request.Query.OrderBy(x => x.Key))
            {
                key.Append($"|{keyName.ToLower()}={value}"); // api/products|category=electronics|page=1
            }
            return key.ToString();
        }
    }
}
