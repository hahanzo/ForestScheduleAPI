using Microsoft.AspNetCore.Mvc.Filters;

namespace ForestSchedule.API.Filters
{
    public class MetadataHeaderFilter : IAsyncResultFilter
    {
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            context.HttpContext.Response.Headers.Append("X-University", "NLTU Ukraine");
            context.HttpContext.Response.Headers.Append("X-Generated-At", DateTime.UtcNow.ToString("O"));

            await next();
        }
    }
}
