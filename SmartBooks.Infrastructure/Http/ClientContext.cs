using Microsoft.AspNetCore.Http;
using SmartBooks.Application.Interfaces;

namespace SmartBooks.Infrastructure.Http;

public class ClientContext: IClientContext
{
    private readonly IHttpContextAccessor _accessor;

    public ClientContext(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }

    public string ClientType =>
        _accessor.HttpContext?.Items["ClientType"]?.ToString() ?? "unknown";

    public bool IsMobile => ClientType == "mobile";
    public bool IsWeb => ClientType == "web";
}
