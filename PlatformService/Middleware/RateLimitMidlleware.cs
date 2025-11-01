
using Microsoft.Extensions.Caching.Memory;

namespace PlatformService.Middleware
{
    public class RateLimitMiddleware
    {
        private const int _limit = 10; // max requests per 10 seconds fixed window
        private static readonly TimeSpan _window = TimeSpan.FromSeconds(60);

        private static readonly MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());

        private readonly RequestDelegate _next;
        private readonly ILogger<RateLimitMiddleware> _logger;

        public RateLimitMiddleware(RequestDelegate next, ILogger<RateLimitMiddleware> logger)
        {
            this._next = next;
            this._logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";

            if (_cache.TryGetValue(ip, out int count))
            {
                if (count >= _limit)
                {
                    context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                    await context.Response.WriteAsync("Too many requests, please try again later.");
                    _logger.LogWarning($"Rate limit exceeded for IP: {ip}");
                    return;
                }
                _cache.Set(ip, count + 1, _window);
            }
            else
            {
                _cache.Set(ip, 1, _window);
            }
            await _next(context);
        }
    }
}