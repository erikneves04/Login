namespace Access.Models.View;

public class LoginView
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class TokenView
{
    public TokenView() { }
    public TokenView(string message, string token, DateTime expiresAt)
    {
        Message = message;
        Token = token;
        ExpiresAt = expiresAt;
    }

    public string Message { get; set; }
    public string Token { get; set; }
    public DateTime ExpiresAt { get; set; }
}

public class TokenDecryptedData
{
    public TokenDecryptedData() { }
    public TokenDecryptedData(Guid userId, string userName, DateTime expiresAt)
    {
        UserId = userId;
        UserName = userName;
        ExpiresAt = expiresAt;
    }

    public Guid UserId { get; set; }
    public string UserName { get; set; }
    public DateTime ExpiresAt { get; set; }
}