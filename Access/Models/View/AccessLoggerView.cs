using Access.Models.Base;

namespace Access.Models.View;

public class AccessLoggerView
{
    public AccessLoggerView() { }
    public AccessLoggerView(Guid id, UserView user, string token, string ipAddress, DateTime createdAt,DateTime expiresAt)
    {
        Id = id;
        User = user;
        Token = token;
        IpAddress = ipAddress;
        CreatedAt = createdAt;
        ExpiresAt = expiresAt;
    }
    public AccessLoggerView(AccessLogger logger)
    {
        UserView user = null;
        if (logger.User != null)
            user = new UserView(logger.User);

        Id = logger.Id;
        User = user;
        Token = logger.Token;
        IpAddress = logger.IpAddress;
        CreatedAt = logger.CreatedAt;
        ExpiresAt = logger.ExpiresAt;
    }

    public Guid Id { get; set; }
    public UserView User { get; set; }
    public string Token { get; set; }
    public string IpAddress { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
}

public class AccessLoggerInsertView
{
    public AccessLoggerInsertView() { }
    public AccessLoggerInsertView(Guid userId, string token, string ipAddress, DateTime expiresAt)
    {
        UserId = userId;
        Token = token;
        IpAddress = ipAddress;
        ExpiresAt = expiresAt;
    }

    public Guid UserId { get; set; }
    public string Token { get; set; }
    public string IpAddress { get; set; }
    public DateTime ExpiresAt { get; set; }
}
