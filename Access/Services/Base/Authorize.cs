using Access.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Access.Services.Base;

public abstract class Authorize : ControllerBase
{
    private readonly IHttpContextAccessor _autorizeContextAccessor;
    private readonly IAccessLoggerServices _authorizeAccessLoggerServices;

    protected Authorize(IHttpContextAccessor contextAccessor, IAccessLoggerServices accessLoggerServices)
    {
        _autorizeContextAccessor = contextAccessor;
        _authorizeAccessLoggerServices = accessLoggerServices;
    }

    protected void ValidateToken()
    {
        var token = GetRequestToken();

        if (string.IsNullOrEmpty(token))
            throw new InvalidOperationException("Token not found");

        try
        {
            if (!_authorizeAccessLoggerServices.IsExpired(token))
                throw new InvalidOperationException("Expired Token");
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(ex.Message);
        }
    }

    private string GetRequestToken()
    {
        string token = _autorizeContextAccessor.HttpContext.Request.Headers["Authorization"];
        return token?.Substring(7);
    }
}
