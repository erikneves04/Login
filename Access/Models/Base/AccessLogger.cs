using Access.Models.View;

namespace Access.Models.Base;

public class AccessLogger : Base
{
    public AccessLogger() { }
    public AccessLogger(Guid userId, string token, string ipAddress, DateTime expiresAt) 
    { 
        UserId = userId;    
        Token = token;
        IpAddress = ipAddress;
        ExpiresAt = expiresAt;
    }
    public AccessLogger(AccessLoggerInsertView data)
    {
        UserId = data.UserId;
        Token = data.Token;
        IpAddress = data.IpAddress;
        ExpiresAt = data.ExpiresAt;
    }

    public Guid UserId { get; set; }
    public User User { get; set; }

    public string Token { get; set; }
    public string IpAddress { get; set; }
    public DateTime ExpiresAt { get; set; }
}
