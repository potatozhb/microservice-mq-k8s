
namespace PlatformService.Middleware
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AuthMiddleware> _logger;

        public AuthMiddleware(RequestDelegate next, ILogger<AuthMiddleware> logger)
        {
            this._next = next;
            this._logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.User.Identity.Name != null)
            {// temp skip check authentication
                if (!(context.User.Identity?.IsAuthenticated ?? false))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("User is unauthorized");
                }
            }
            await _next(context);
        }
    }
}