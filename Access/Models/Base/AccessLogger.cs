using Access.Models.View;

namespace Access.Models.Base;

public class AccessLogger : Base
{
    public AccessLogger() 
    {
        UpdateExpireState();
    }
    public AccessLogger(Guid userId, string token, string ipAddress, DateTime expiresAt, bool isExpired) 
    { 
        UserId = userId;    
        Token = token;
        IpAddress = ipAddress;
        ExpiresAt = expiresAt;
        IsExpired = isExpired;

        UpdateExpireState();
    }
    public AccessLogger(AccessLoggerInsertView data)
    {
        UserId = data.UserId;
        Token = data.Token;
        IpAddress = data.IpAddress;
        ExpiresAt = data.ExpiresAt;
        IsExpired = false;
    }

    public Guid UserId { get; set; }
    public User User { get; set; }

    public string Token { get; set; }
    public string IpAddress { get; set; }
    public DateTime ExpiresAt { get; set; }
    public bool IsExpired { get; set; }

    private void UpdateExpireState()
    {
        if (IsExpired)
            return;

        if(ExpiresAt >= DateTime.Now)
            IsExpired = true;
    }
}
