namespace SmartBooks.Api.Middleware;

public class ClientTypeMiddleware
{

    private readonly RequestDelegate _next;

    public ClientTypeMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        var userAgent = context.Request.Headers["User-Agent"].ToString().ToLower();

        var clientType = userAgent switch
        {
            var ua when ua.Contains("android") || ua.Contains("iphone") => "mobile",
            var ua when ua.Contains("mozilla") || ua.Contains("chrome") => "web",
            _ => "unknown"
        };

        // Lo guardas en los Items para usarlo en cualquier parte
        context.Items["ClientType"] = clientType;

        await _next(context);
    }
}
