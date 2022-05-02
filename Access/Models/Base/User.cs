using Access.Models.View;

namespace Access.Models.Base;

public class User : Base
{
    public User() 
    {
        AccessBlockUpdate();
    }
    public User(string name, string email,string password)
    { 
        Name = name;
        Email = email;
        Password = password;
        IsActive = true;

        AccessBlockUpdate();
    }
    public User(UserInsertView user)
    {
        Name = user.Name;
        Email = user.Email;
        Password = user.Password;
        IsActive = true;

        AccessBlockUpdate();
    }

    public bool IsActive { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

    public static readonly int MaxAccessAttempts = 4;
    private int AccessAttemptsCount { get; set; } = 0;
    public bool AccessIsBlocked { get; private set; } = false;
    public DateTime BlockExpiresAt { get; private set; } = DateTime.MinValue;

    public void SetPassword(string password) => Password = password;
    public bool IsCorrectPassword(string password) 
    {
        if (AccessIsBlocked)
            return false;

        var isCorrect = (Password == password);
        if (isCorrect == false)
            AccessAttemptsCount++;
        
        if (AccessAttemptsCount == MaxAccessAttempts)
        {
            AccessIsBlocked = true;
            BlockExpiresAt = DateTime.Now.AddMinutes(30);
        }

        return isCorrect;
    }
    private void AccessBlockUpdate()
    {
        if (BlockExpiresAt == DateTime.MinValue)
            return;

        if (BlockExpiresAt >= DateTime.UtcNow)
            return;

        BlockExpiresAt = DateTime.MinValue;
        AccessAttemptsCount = 0;
        AccessIsBlocked = false;
    }
}