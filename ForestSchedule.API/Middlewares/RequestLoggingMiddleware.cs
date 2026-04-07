using System.Diagnostics;

namespace ForestSchedule.API.Middlewares
{
    public class RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            var traceId = Activity.Current?.Id ?? context.TraceIdentifier;

            logger.LogInformation("[{TraceId}] Request received: {Method} {Path}",
                traceId, context.Request.Method, context.Request.Path);

            var watch = Stopwatch.StartNew();

            await next(context);

            watch.Stop();

            logger.LogInformation("[{TraceId}] Request done: {Method} {Path} | Status: {StatusCode} | Time: {ElapsedMs} ms",
                traceId, context.Request.Method, context.Request.Path, context.Response.StatusCode, watch.ElapsedMilliseconds);
        }
    }
}
