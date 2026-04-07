using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;

namespace ForestSchedule.API.Filters
{
    public class CacheResourceFilter (IMemoryCache cache) : IAsyncResourceFilter
    {
        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            var cacheKey = context.HttpContext.Request.Path.ToString();

            if (cache.TryGetValue(cacheKey, out object? cachedResponse))
            {
                Console.WriteLine("Response was served from cache");
                context.Result = new OkObjectResult(cachedResponse);
                return;
            }

            var executedContext = await next();

            if (executedContext.Result is OkObjectResult okResult)
            {
                Console.WriteLine("Response was cached");
                cache.Set(cacheKey, okResult.Value, TimeSpan.FromMinutes(5));
            }
        }
    }
}
