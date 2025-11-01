
using System.Text.Json;
using Grpc.Core;

namespace PlatformService.Middleware
{
    public class JsonFormatMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<JsonFormatMiddleware> _logger;

        public JsonFormatMiddleware(RequestDelegate next, ILogger<JsonFormatMiddleware> logger)
        {
            this._next = next;
            this._logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.ContentType?.Contains("application/json", StringComparison.OrdinalIgnoreCase) == true &&
                (HttpMethods.IsPost(context.Request.Method) ||
                 HttpMethods.IsPut(context.Request.Method) ||
                 HttpMethods.IsPatch(context.Request.Method)))
            {
                context.Request.EnableBuffering();
                using var reader = new StreamReader(context.Request.Body, leaveOpen: true); //auto disposed
                var body = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;
                if (!string.IsNullOrEmpty(body))
                {
                    try
                    {
                        var jsondoc = JsonDocument.Parse(body);
                        var root = jsondoc.RootElement;
                        var normalizedJson = JsonSerializer.Serialize(root, new JsonSerializerOptions
                        {
                            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                        });

                        var bytes = System.Text.Encoding.UTF8.GetBytes(normalizedJson);
                        context.Request.Body = new MemoryStream(bytes);
                    }
                    catch (JsonException ex)
                    {
                        _logger.LogError($"Bad request: {ex.Message}");
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        await context.Response.WriteAsJsonAsync($"Invalid Json format: {ex.Message}");
                    }
                }
            }
        }
    }
}