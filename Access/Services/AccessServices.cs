using Access.Interfaces.Services;
using Access.Models.View;
using System.Security.Cryptography;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace Access.Services;

public class AccessServices : IAccessServices
{
    private readonly IAccessLoggerServices _loggerServices;
    private readonly IConfiguration _config;
    private readonly IUserServices _userServices;
    private readonly IHttpContextAccessor _httpContextAccessor;

    // Don't change the keys
    public static readonly Dictionary<string, string> MainPermissions = new() { { "Admin", "admin" }, { "Common", "common" } };

    public AccessServices(IAccessLoggerServices loggerServices, IUserServices userServices, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        _loggerServices = loggerServices;
        _userServices = userServices;
        _config = configuration;
        _httpContextAccessor = httpContextAccessor;
    }

    public TokenView Login(LoginView login)
    {
        ValidateLoginParams(login);

        var user = _userServices.Access(login.Email, login.Password);

        var ipAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
        var token = CreateToken(user.Id, user.Name);

        _loggerServices.Insert(new AccessLoggerInsertView(user.Id, token.Token, ipAddress, token.ExpiresAt));
        return token;
    }

    private static void ValidateLoginParams(LoginView login)
    {
        if (string.IsNullOrEmpty(login.Email))
            throw new InvalidDataException("Email is required");

        if (string.IsNullOrEmpty(login.Password))
            throw new InvalidDataException("Password is required");
    }

    private TokenView CreateToken(Guid userId, string userName)
    {
        var expirationConfig = _config["TokenConfiguration:ExpireHours"];
        var expiration = DateTime.UtcNow.AddHours(double.Parse(expirationConfig));

        var claims = new List<Claim>()
        {
            new Claim("userId", userId.ToString()),
            new Claim("userName", userName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("expiresAt", expiration.ToString()),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var credenciais = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["TokenConfiguration:Issuer"],
            audience: _config["TokenConfiguration:Audience"],
            claims: claims,
            expires: expiration,
            signingCredentials: credenciais);

        return new TokenView()
        {
            Message = "authorized access",
            ExpiresAt = expiration,
            Token = new JwtSecurityTokenHandler().WriteToken(token),
        };
    }

    public TokenDecryptedData GetSessionData()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        ClaimsPrincipal user = httpContext.User;

        if (user == null)
            return null;

        var userId = Guid.Parse(user.FindFirst("userId")?.Value);
        var userName = user.FindFirst("userName")?.Value;
        var expiresAt = DateTime.Parse(user.FindFirst("expiresAt")?.Value);

        return new TokenDecryptedData(userId, userName, expiresAt);
    }
}