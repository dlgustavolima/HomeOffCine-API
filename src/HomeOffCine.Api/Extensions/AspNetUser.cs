using HomeOffCine.Business.Interfaces.Service;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace HomeOffCine.Api.Extensions;

public class AspNetUser : IUser
{
    private readonly IHttpContextAccessor _contextAccessor;

    public AspNetUser(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    public string GetUserName()
    {
        var userName = _contextAccessor.HttpContext?.User.FindFirst("username")?.Value;
        if (!string.IsNullOrEmpty(userName)) return userName;

        userName = _contextAccessor.HttpContext?.User.Identity?.Name;
        if (!string.IsNullOrEmpty(userName)) return userName;

        userName = _contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name)?.Value;
        if (!string.IsNullOrEmpty(userName)) return userName;

        userName = _contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.GivenName)?.Value;
        if (!string.IsNullOrEmpty(userName)) return userName;

        return string.Empty;
    }

    public Guid GetUserId()
    {
        if (!IsAuthenticated()) return Guid.Empty;

        var claim = _contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(claim))
            claim = _contextAccessor.HttpContext?.User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

        return claim is null ? Guid.Empty : Guid.Parse(claim);
    }

    public bool IsAuthenticated()
    {
        return _contextAccessor.HttpContext?.User.Identity is { IsAuthenticated: true };
    }

    public bool IsInRole(string role)
    {
        return _contextAccessor.HttpContext != null && _contextAccessor.HttpContext.User.IsInRole(role);
    }
}
